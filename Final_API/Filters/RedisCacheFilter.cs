using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace Final_API.Filters;

public class RedisCacheFilter(IDistributedCache distributedCache, ILogger<RedisCacheFilter> logger)
  : IAsyncActionFilter {
  private const int DurationInSeconds = 120;

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
    var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
    string? cachedResponse = null;

    try {
      cachedResponse = await distributedCache.GetStringAsync(cacheKey);
    }
    catch (Exception ex) {
      // Log the error and gracefully fall back
      logger.LogWarning($"Redis is unavailable. Proceeding without cache. Error: {ex.Message}");
    }

    if (!string.IsNullOrEmpty(cachedResponse)) {
      context.Result = new ContentResult {
        Content = cachedResponse,
        ContentType = "application/json",
        StatusCode = 200
      };
      return;
    }

    var executedContext = await next();

    if (executedContext is { Result: ObjectResult { Value: not null } result, HttpContext.Response.StatusCode: 200 }) {
      var responseBody = SerializeResult(result.Value);

      try {
        var options = new DistributedCacheEntryOptions {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(DurationInSeconds)
        };
        await distributedCache.SetStringAsync(cacheKey, responseBody, options);
      }
      catch (Exception ex) {
        // Log the error and proceed without cache
        logger.LogWarning($"Failed to store data in Redis. Error: {ex.Message}");
      }
    }
  }

  private static string GenerateCacheKeyFromRequest(HttpRequest request) {
    return $"{request.Path}_{request.QueryString}";
  }

  private static string SerializeResult(object result) {
    return System.Text.Json.JsonSerializer.Serialize(result);
  }
}