using Final_UI.Helpers.Exceptions;
using System.Net;
using System.Text.Json;
using Final_UI.Models.Responses;

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
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)ex.Response.StatusCode;

    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) {
      var returnUrl = context.Request.Path.ToString();
      context.Response.Redirect($"/Account/Login?returnUrl={returnUrl}");
      return;
    }

    ErrorResponse errorResponse;
    try {
      var options = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
      };
      var responseContent = await ex.Response.Content.ReadAsStringAsync();
      errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, options)!;
      errorResponse!.StatusCode = context.Response.StatusCode;
    }
    catch (Exception deserializationEx) {
      errorResponse = new ErrorResponse {
        StatusCode = context.Response.StatusCode,
        Message = ex.Response.ReasonPhrase ?? "An error occurred processing your request.",
        Errors = [
          new ErrorResponseItem { Key = "DeserializationError", Message = deserializationEx.Message }
        ]
      };
    }

    var result = JsonSerializer.Serialize(errorResponse);
    await context.Response.WriteAsync(result);
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception ex) {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var errorResponse = new ErrorResponse {
      StatusCode = context.Response.StatusCode,
      Message = "An internal server error has occurred.",
      Errors = [
        new ErrorResponseItem { Key = "Exception", Message = ex.Message }
      ]
    };

    var result = JsonSerializer.Serialize(errorResponse);
    await context.Response.WriteAsync(result);
  }
}