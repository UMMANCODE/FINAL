namespace Final_UI.Controllers;
public class ErrorController : Controller {
  public new IActionResult Unauthorized() {
    return View();
  }

  public new IActionResult NotFound() {
    return View();
  }

  public new IActionResult BadRequest() {
    return View();
  }

  public IActionResult InternalServerError() {
    return View();
  }

  public IActionResult Forbidden() {
    return View();
  }
}
