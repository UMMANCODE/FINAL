using Final_UI.Helpers.Enums;
using Final_UI.Helpers.Filters;
using Final_UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
//[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class HomeController(IDataService dataService) : Controller {
  public IActionResult Index() {
    return View();
  }

  public IActionResult Error(int? statusCode = null) {
    if (statusCode.HasValue) {
      return statusCode switch {
        401 => RedirectToAction("Unauthorized", "Error"),
        403 => RedirectToAction("Forbidden", "Error"),
        404 => RedirectToAction("NotFound", "Error"),
        400 => RedirectToAction("BadRequest", "Error"),
        _ => RedirectToAction("InternalServerError", "Error")
      };
    }

    return RedirectToAction("InternalServerError", "Error");
  }

  public async Task<IActionResult> Comments() {
    var comments = await dataService.GetComments();
    return View(comments);
  }

  public async Task<IActionResult> Orders() {
    var orders = await dataService.GetOrders();
    return View(orders);
  }

  public async Task<IActionResult> UpdateOrderStatus(int id, string status) {
    var orderStatus = Enum.Parse<OrderStatus>(status);
    await dataService.UpdateOrderStatus(id, orderStatus);
    return RedirectToAction("Orders");
  }

  public async Task<IActionResult> UpdateCommentStatus(int id, string status) {
    var commentStatus = Enum.Parse<CommentStatus>(status);
    await dataService.UpdateCommentStatus(id, commentStatus);
    return RedirectToAction("Comments");
  }
}
