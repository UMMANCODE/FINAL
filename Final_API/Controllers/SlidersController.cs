namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlidersController(ISliderService sliderService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await sliderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/all")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetAll() {
    var response = await sliderService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await sliderService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateSlider([FromForm] SliderCreateDto sliderDto) {
    var response = await sliderService.Create(sliderDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateSlider(int id, [FromForm] SliderUpdateDto sliderDto) {
    var response = await sliderService.Update(id, sliderDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteSlider(int id) {
    var response = await sliderService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [Authorize(Roles = "Member")]
  [HttpGet("user")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await sliderService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/all")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetAll() {
    var response = await sliderService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpGet("user/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await sliderService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPost("user")]
  public async Task<IActionResult> UserCreateSlider([FromForm] SliderCreateDto sliderDto) {
    var response = await sliderService.Create(sliderDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpPut("user/{id:int}")]
  public async Task<IActionResult> UserUpdateSlider(int id, [FromForm] SliderUpdateDto sliderDto) {
    var response = await sliderService.Update(id, sliderDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Member")]
  [HttpDelete("user/{id:int}")]
  public async Task<IActionResult> UserDeleteSlider(int id) {
    var response = await sliderService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }
}