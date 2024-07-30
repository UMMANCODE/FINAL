using Final_Business.DTOs.General;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController(IFeatureService featureService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin")]
  [HttpGet("admin")]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await featureService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var data = await featureService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var data = await featureService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateFeature([FromForm] FeatureCreateDto featureDto) {
    var id = await featureService.Create(featureDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateFeature(int id, [FromForm] FeatureUpdateDto featureDto) {
    await featureService.Update(id, featureDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteFeature(int id) {
    await featureService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  // Client routes
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await featureService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var data = await featureService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var data = await featureService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpPost("user")]
  public async Task<IActionResult> UserCreateFeature([FromForm] FeatureCreateDto featureDto) {
    var id = await featureService.Create(featureDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateFeature(int id, [FromForm] FeatureUpdateDto featureDto) {
    await featureService.Update(id, featureDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteFeature(int id) {
    await featureService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }
}