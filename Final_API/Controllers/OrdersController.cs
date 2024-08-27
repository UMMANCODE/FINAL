using Final_Core.Enums;

namespace Final_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin")]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await orderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var response = await orderService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await orderService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPut("admin/{id:int}/status")]
  public async Task<IActionResult> AdminUpdateStatus(int id, OrderStatus status) {
    var response = await orderService.UpdateStatus(id, status);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [Authorize(Roles = "Member")]
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await orderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var response = await orderService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await orderService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPost("user")]
  public async Task<IActionResult> UserCreateOrder([FromForm] OrderCreateDto orderDto) {
    var response = await orderService.Create(orderDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPut("user/{id:int}/status")]
  public async Task<IActionResult> UserUpdateStatus(int id, OrderStatus status) {
    var response = await orderService.UpdateStatus(id, status);
    return StatusCode(response.StatusCode, response);
  }
}