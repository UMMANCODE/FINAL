using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HouseImagesController(IHouseImageService houseImageService) : ControllerBase {
  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  public async Task<IActionResult> AdminGetById(int id) {
    var data = await houseImageService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }

  [HttpGet("user/{id:int}")]
  public async Task<IActionResult> UserGetById(int id) {
    var data = await houseImageService.GetById(id);
    return StatusCode(StatusCodes.Status200OK, data);
  }
}
