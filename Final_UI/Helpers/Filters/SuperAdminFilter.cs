using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Final_UI.Helpers.Filters;

public class SuperAdminFilter : IActionFilter {
  public void OnActionExecuted(ActionExecutedContext context) { }

  public void OnActionExecuting(ActionExecutingContext context) {
    var controller = context.Controller as ControllerBase;
    var token = GetTokenFromRequest(context.HttpContext);

    token = token?.Replace("Bearer ", "");

    if (token == null || !IsUserAuthorized(token)) {
      context.Result = controller?.RedirectToAction(
        actionName: "Unauthorized",
        controllerName: "Error",
        new { returnUrl = context.HttpContext.Request.Path }
      );
    }
  }

  private string? GetTokenFromRequest(HttpContext httpContext) {
    // Assuming token is stored in a cookie
    return httpContext.Request.Cookies["token"];
  }

  private bool IsUserAuthorized(string token) {
    var handler = new JwtSecurityTokenHandler();

    if (handler.ReadToken(token) is not JwtSecurityToken jwtToken)
      return false;

    var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
    return roles.Contains("SuperAdmin");
  }
}