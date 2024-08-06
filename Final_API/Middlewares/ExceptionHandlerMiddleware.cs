using Final_Business.Exceptions;
using Final_Business.Helpers;

namespace Final_API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next) {
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next.Invoke(context);
    }
    catch (Exception ex) {
      var response = new BaseResponse(500, ex.Message, null, []);
      context.Response.StatusCode = 500;

      if (ex is RestException rex) {
        response.StatusCode = rex.Code;
        response.Message = rex.Message;
        response.Errors = rex.Errors;
        context.Response.StatusCode = rex.Code;
      }

      await context.Response.WriteAsJsonAsync(response);
    }
  }
}