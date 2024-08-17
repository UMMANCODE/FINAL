using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HouseImagesController(IHouseImageService houseImageService) : ControllerBase {
  [Authorize(Roles = "Admin")]
  [HttpGet("admin/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await houseImageService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("user/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await houseImageService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }
}
