using Final_Business.DTOs.Admin;
using Final_Business.DTOs.User;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HousesController(IAdminHouseService adminHouseService, IUserHouseService userHouseService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin")]
  [HttpGet("admin")]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await adminHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var data = await adminHouseService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var data = await adminHouseService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateHouse([FromForm] AdminHouseCreateDto houseDto) {
    var id = await adminHouseService.Create(houseDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateHouse(int id, [FromForm] AdminHouseUpdateDto houseDto) {
    await adminHouseService.Update(id, houseDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteHouse(int id) {
    await adminHouseService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  // Client routes
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var data = await userHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var data = await userHouseService.GetAll();
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var data = await userHouseService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpPost("user")]
  public async Task<IActionResult> UserCreateHouse([FromForm] UserHouseCreateDto houseDto) {
    var id = await userHouseService.Create(houseDto);
    return StatusCode(StatusCodes.Status201Created, new { id });
  }

  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateHouse(int id, [FromForm] UserHouseUpdateDto houseDto) {
    await userHouseService.Update(id, houseDto);
    return StatusCode(StatusCodes.Status204NoContent);
  }

  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteHouse(int id) {
    await userHouseService.Delete(id);
    return StatusCode(StatusCodes.Status204NoContent);
  }
}