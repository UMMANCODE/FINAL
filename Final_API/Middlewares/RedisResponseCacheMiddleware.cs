using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Final_API.Middlewares {
  public class RedisResponseCacheMiddleware(
    RequestDelegate next,
    IDistributedCache distributedCache,
    ILogger<RedisResponseCacheMiddleware> logger) {
    private static bool _circuitOpen;
    private static DateTime _circuitResetTime = DateTime.MinValue;
    private static int _failureCount;
    private const int FailureThreshold = 1;
    private static readonly TimeSpan CircuitOpenDuration = TimeSpan.FromMinutes(1);

    public async Task InvokeAsync(HttpContext context) {
      var cacheKey = $"{context.Request.Path}_{context.Request.QueryString}";

      if (_circuitOpen && DateTime.UtcNow < _circuitResetTime) {
        logger.LogWarning("Circuit is open, skipping Redis.");
        await next(context);
        return;
      }

      string? cachedResponse;

      try {
        cachedResponse = await distributedCache.GetStringAsync(cacheKey);
        _failureCount = 0;
      }
      catch (Exception ex) {
        _failureCount++;

        if (_failureCount >= FailureThreshold) {
          _circuitOpen = true;
          _circuitResetTime = DateTime.UtcNow.Add(CircuitOpenDuration);
          logger.LogError($"Circuit opened due to repeated Redis failures. {ex.Message}");
        }
        else {
          logger.LogWarning($"Redis failure. Attempt {_failureCount}. Error: {ex.Message}");
        }

        await next(context);
        return;
      }

      if (!string.IsNullOrEmpty(cachedResponse)) {
        context.Response.Headers.Append("X-Cache", "HIT");
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(cachedResponse);
        return;
      }

      var originalBodyStream = context.Response.Body;
      using var newBodyStream = new MemoryStream();
      context.Response.Body = newBodyStream;

      try {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status200OK) {
          newBodyStream.Seek(0, SeekOrigin.Begin);
          var responseText = await new StreamReader(newBodyStream).ReadToEndAsync();

          try {
            await distributedCache.SetStringAsync(cacheKey, responseText, new DistributedCacheEntryOptions {
              AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
          }
          catch (Exception ex) {
            logger.LogWarning($"Failed to store data in Redis: {ex.Message}");
          }

          newBodyStream.Seek(0, SeekOrigin.Begin);
          await newBodyStream.CopyToAsync(originalBodyStream);
        }
        else {
          newBodyStream.Seek(0, SeekOrigin.Begin);
          await newBodyStream.CopyToAsync(originalBodyStream);
        }
      }
      catch (Exception ex) {
        logger.LogError(ex, "An error occurred while processing the request.");
        throw;
      }
      finally {
        context.Response.Body = originalBodyStream; // Ensure the original stream is restored
      }
    }
  }
}
