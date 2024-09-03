namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HousesController(IAdminHouseService adminHouseService, IUserHouseService userHouseService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await adminHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/all")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetAll() {
    var response = await adminHouseService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await adminHouseService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateHouse([FromForm] AdminHouseCreateDto houseDto) {
    var response = await adminHouseService.Create(houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateHouse(int id, [FromForm] AdminHouseUpdateDto houseDto) {
    var response = await adminHouseService.Update(id, houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteHouse(int id) {
    var response = await adminHouseService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [Authorize(Roles = "Member")]
  [HttpGet("user")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await userHouseService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/all")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetAll() {
    var response = await userHouseService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await userHouseService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPost("user")]
  public async Task<IActionResult> UserCreateHouse([FromForm] UserHouseCreateDto houseDto) {
    var response = await userHouseService.Create(houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateHouse(int id, [FromForm] UserHouseUpdateDto houseDto) {
    var response = await userHouseService.Update(id, houseDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteHouse(int id) {
    var response = await userHouseService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }
  
  [Authorize(Roles = "Member")]
  [HttpGet("user/filter")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserFilter([FromQuery] PropertyStatus? status = null,
    [FromQuery] PropertyType? type = null, [FromQuery] PropertyState? state = null) {
    var response = await userHouseService.Filter(status, type, state);
    return StatusCode(response.StatusCode, response);
  }
}
