using Final_Business.DTOs.General;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController(ICommentService commentService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin")]
  [HttpGet("admin")]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await commentService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var data = await commentService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var data = await commentService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateComment([FromForm] CommentCreateDto commentDto) {
    var id = await commentService.Create(commentDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  // Client routes
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await commentService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var data = await commentService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var data = await commentService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpPost("user")]
  public async Task<IActionResult> UserCreateComment([FromForm] CommentCreateDto commentDto) {
    var id = await commentService.Create(commentDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }
}