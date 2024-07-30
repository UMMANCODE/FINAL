using Final_Business.DTOs.General;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlidersController(ISliderService sliderService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin")]
  [HttpGet("admin")]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await sliderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var data = await sliderService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var data = await sliderService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateSlider([FromForm] SliderCreateDto sliderDto) {
    var id = await sliderService.Create(sliderDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateSlider(int id, [FromForm] SliderUpdateDto sliderDto) {
    await sliderService.Update(id, sliderDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteSlider(int id) {
    await sliderService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  // Client routes
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await sliderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var data = await sliderService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var data = await sliderService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpPost("user")]
  public async Task<IActionResult> UserCreateSlider([FromForm] SliderCreateDto sliderDto) {
    var id = await sliderService.Create(sliderDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateSlider(int id, [FromForm] SliderUpdateDto sliderDto) {
    await sliderService.Update(id, sliderDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteSlider(int id) {
    await sliderService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }
}