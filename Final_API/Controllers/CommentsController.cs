using Final_Core.Enums;

namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController(ICommentService commentService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await commentService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/all")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> AdminGetAll() {
    var response = await commentService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await commentService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPut("admin/{id:int}/status")]
  public async Task<IActionResult> AdminUpdateStatus(int id, CommentStatus status) {
    var response = await commentService.UpdateStatus(id, status);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [Authorize(Roles = "Member")]
  [HttpGet("user")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await commentService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/all")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> UserGetAll() {
    var response = await commentService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/{id:int}")]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await commentService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPost("user")]
  public async Task<IActionResult> UserCreateComment([FromForm] CommentCreateDto commentDto) {
    var response = await commentService.Create(commentDto);
    return StatusCode(response.StatusCode, response);
  }
}