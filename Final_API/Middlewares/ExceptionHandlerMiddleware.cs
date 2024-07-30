using Final_Business.Exceptions;

namespace Final_API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next) {
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next.Invoke(context);
    }
    catch (Exception ex) {
      var message = ex.Message;
      var errors = new List<RestExceptionError>();
      context.Response.StatusCode = 500;

      if (ex is RestException rex) {
        message = rex.Message;
        errors = rex.Errors;
        context.Response.StatusCode = rex.Code;
      }

      await context.Response.WriteAsJsonAsync(new { message, errors });
    }
  }
}