namespace Final_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HouseImagesController(IHouseImageService houseImageService) : ControllerBase {

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await houseImageService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await houseImageService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }
}
