using Microsoft.AspNetCore.Mvc;

namespace Final_UI.Controllers;
public class HomeController : Controller {
  public IActionResult Index() {
    return View();
  }

  public IActionResult Error() {
    return View();
  }
}
