using Final_UI.Helpers.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class HomeController(
  IConfiguration configuration, IDataService dataService, IUserService userService,
  ICrudService crudService, IHubContext<NotificationHub> hubContext, IExcelReportService reportService
  ) : Controller {
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;
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

  public IActionResult Charts() {
    return View();
  }

  public async Task<IActionResult> Comments() {
    var comments = await dataService.GetComments();
    return View(comments);
  }

  public async Task<IActionResult> GenerateReportCommentsOrders() {
    var comments = await dataService.GetComments();
    var orders = await dataService.GetOrders();

    var datasets = new List<(object data, string sheetName)> {
      (comments ?? [], "Comments"),
      (orders ?? [], "Orders")
    };

    var report = reportService.GenerateReport(datasets);
    return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CommentsAndOrders.xlsx");
  }

  public async Task<IActionResult> GenerateReportUsers() {
    var currentUserRole = userService.GetRole();

    if (currentUserRole == "SuperAdmin") {
      var users = await userService.GetUsers();

      var members = users?.Where(u => u.Roles.Contains("Member")).ToList();
      var admins = users?.Where(u => u.Roles.Contains("Admin") || u.Roles.Contains("SuperAdmin")).ToList();

      var datasets = new List<(object data, string sheetName)> {
        (members ?? [], "Members"),
        (admins ?? [], "Admins")
      };

      var report = reportService.GenerateReport(datasets);
      return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MembersAndAdmins.xlsx");
    }

    if (currentUserRole == "Admin") {
      var users = await userService.GetUsers();

      var members = users?.Where(u => u.Roles.Contains("Member")).ToList();

      var datasets = new List<(object data, string sheetName)> {
        (members ?? [], "Members")
      };

      var report = reportService.GenerateReport(datasets);
      return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Members.xlsx");
    }

    return RedirectToAction("Forbidden", "Error");
  }

  public async Task<IActionResult> GenerateReportCrud() {

    var features = await crudService.GetAsync<List<FeatureResponse>>($"{_apiUrl}/Features/admin/all");
    var sliders = await crudService.GetAsync<List<SliderResponse>>($"{_apiUrl}/Sliders/admin/all");
    var houses = await crudService.GetAsync<List<HouseResponse>>($"{_apiUrl}/Houses/admin/all");
    var discounts = await crudService.GetAsync<List<DiscountResponse>>($"{_apiUrl}/trait/Discounts/all");

    var datasets = new List<(object data, string sheetName)> {
        (features ?? [], "Features"),
        (discounts ?? [], "Discounts"),
        (sliders ?? [], "Sliders"),
        (houses ?? [], "Houses")
      };

    var report = reportService.GenerateReport(datasets);
    return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Crud.xlsx");
  }

  public async Task<IActionResult> Orders() {
    var orders = await dataService.GetOrders();
    return View(orders);
  }

  public async Task<IActionResult> UpdateOrderStatus(int id, string status, string userId) {
    var orderStatus = Enum.Parse<OrderStatus>(status);
    await dataService.UpdateOrderStatus(id, orderStatus);
    await hubContext.Clients.User(userId).SendAsync("notify", $"Your order has been {status}!");
    return RedirectToAction("Orders");
  }

  public async Task<IActionResult> UpdateCommentStatus(int id, string status, string userId) {
    var commentStatus = Enum.Parse<CommentStatus>(status);
    await dataService.UpdateCommentStatus(id, commentStatus);
    await hubContext.Clients.User(userId).SendAsync("notify", $"Your comment has been {status}!");
    return RedirectToAction("Comments");
  }

  public async Task<IActionResult> Members() {
    var all = await userService.GetUsers();
    var members = all?.Where(u => u.Roles.Contains("Member")).ToList();
    return View(members);
  }

  [ServiceFilter(typeof(SuperAdminFilter))]
  public async Task<IActionResult> Admins() {
    var all = await userService.GetUsers();
    var admins = all?.Where(u => u.Roles.Contains("Admin") || u.Roles.Contains("SuperAdmin")).ToList();
    return View(admins);
  }
}
