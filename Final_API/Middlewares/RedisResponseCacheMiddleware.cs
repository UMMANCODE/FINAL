using Microsoft.Extensions.Caching.Distributed;

namespace Final_API.Middlewares;

public class RedisResponseCacheMiddleware(
  RequestDelegate next,
  IDistributedCache distributedCache,
  ILogger<RedisResponseCacheMiddleware> logger) {
  private static bool _circuitOpen;
  private static DateTime _circuitResetTime = DateTime.MinValue;
  private static int _failureCount;
  private const int FailureThreshold = 1; // Number of failures before opening the circuit
  private static readonly TimeSpan CircuitOpenDuration = TimeSpan.FromMinutes(1); // Duration to keep the circuit open

  public async Task InvokeAsync(HttpContext context) {
    var cacheKey = $"{context.Request.Path}_{context.Request.QueryString}";

    if (_circuitOpen && DateTime.UtcNow < _circuitResetTime) {
      // Circuit is open, skip Redis
      logger.LogWarning("Circuit is open, skipping Redis.");
      await next(context);
      return;
    }

    string? cachedResponse;

    try {
      cachedResponse = await distributedCache.GetStringAsync(cacheKey);
      _failureCount = 0; // Reset the failure count on a successful operation
    }
    catch (Exception ex) {
      _failureCount++;

      if (_failureCount >= FailureThreshold) {
        // Open the circuit
        _circuitOpen = true;
        _circuitResetTime = DateTime.UtcNow.Add(CircuitOpenDuration);
        logger.LogError($"Circuit opened due to repeated Redis failures. {ex.Message}");
      }
      else {
        logger.LogWarning($"Redis failure. Attempt {_failureCount}. Error: {ex.Message}");
      }

      // Proceed without cache
      await next(context);
      return;
    }

    if (!string.IsNullOrEmpty(cachedResponse)) {
      context.Response.Headers.Append("X-Cache", "HIT");
      context.Response.ContentType = "application/json";
      await context.Response.WriteAsync(cachedResponse);
      return;
    }

    // No cached response, proceed and capture response
    var originalResponseBodyStream = context.Response.Body;
    using var responseBody = new MemoryStream();
    context.Response.Body = responseBody;

    await next(context);

    if (context.Response.StatusCode == 200) {
      context.Response.Body.Seek(0, SeekOrigin.Begin);
      var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();

      try {
        await distributedCache.SetStringAsync(cacheKey, responseText, new DistributedCacheEntryOptions {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
      }
      catch (Exception ex) {
        logger.LogWarning($"Failed to store data in Redis: {ex.Message}");
      }

      context.Response.Body.Seek(0, SeekOrigin.Begin);
      await responseBody.CopyToAsync(originalResponseBodyStream);
    }
  }
}