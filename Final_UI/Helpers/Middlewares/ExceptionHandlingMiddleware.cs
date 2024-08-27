using System.Net;

namespace Final_UI.Helpers.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next(context);
    }
    catch (HttpResponseException ex) {
      logger.LogError($"HttpResponseException caught: {ex.Message}");
      await HandleHttpResponseExceptionAsync(context, ex);
    }
    catch (Exception ex) {
      logger.LogError($"Unhandled exception caught: {ex.Message}");
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleHttpResponseExceptionAsync(HttpContext context, HttpResponseException ex) {
    context.Response.ContentType = "text/html";
    context.Response.StatusCode = (int)ex.Response.StatusCode;

    var statusCode = context.Response.StatusCode;

    if (statusCode == (int)HttpStatusCode.Unauthorized) {
      var returnUrl = context.Request.Path.ToString();
      context.Response.Redirect($"/Account/Login?returnUrl={returnUrl}");
      return;
    }

    var redirectUrl = statusCode switch {
      (int)HttpStatusCode.Forbidden => "/Error/Forbidden",
      (int)HttpStatusCode.NotFound => "/Error/NotFound",
      (int)HttpStatusCode.BadRequest => "/Error/BadRequest",
      _ => "/Error/InternalServerError"
    };

    context.Response.Redirect(redirectUrl);
    await Task.CompletedTask;
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception ex) {
    context.Response.ContentType = "text/html";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    const string redirectUrl = "/Error/InternalServerError";
    context.Response.Redirect(redirectUrl);

    await Task.CompletedTask;
  }
}
