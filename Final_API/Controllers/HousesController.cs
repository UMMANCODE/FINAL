using Azure;
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
    var response = await adminHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/all")]
  public async Task<IActionResult> AdminGetAll() {
    var response = await adminHouseService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await adminHouseService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateHouse([FromForm] AdminHouseCreateDto houseDto) {
    var response = await adminHouseService.Create(houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateHouse(int id, [FromForm] AdminHouseUpdateDto houseDto) {
    var response = await adminHouseService.Update(id, houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteHouse(int id) {
    var response = await adminHouseService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [HttpGet("user")]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await userHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("user/all")]
  public async Task<IActionResult> UserGetAll() {
    var response = await userHouseService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await userHouseService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("user")]
  public async Task<IActionResult> UserCreateHouse([FromForm] UserHouseCreateDto houseDto) {
    var response = await userHouseService.Create(houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateHouse(int id, [FromForm] UserHouseUpdateDto houseDto) {
    var response = await userHouseService.Update(id, houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteHouse(int id) {
    var response = await userHouseService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("user/{id:int}/buy")]
  public async Task<IActionResult> UserBuyHouse(int id, string ownerId) {
    var response = await userHouseService.Buy(id, ownerId);
    return StatusCode(response.StatusCode, response);
  }
}