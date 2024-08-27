namespace Final_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeaturesController(IFeatureService featureService) : ControllerBase {
  // Admin routes
  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await featureService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/all")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetAll() {
    var response = await featureService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("admin/{id:int}")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> AdminGetById(int id) {
    var response = await featureService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPost("admin")]
  public async Task<IActionResult> AdminCreateFeature([FromForm] FeatureCreateDto featureDto) {
    var response = await featureService.Create(featureDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpPut("admin/{id:int}")]
  public async Task<IActionResult> AdminUpdateFeature(int id, [FromForm] FeatureUpdateDto featureDto) {
    var response = await featureService.Update(id, featureDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpDelete("admin/{id:int}")]
  public async Task<IActionResult> AdminDeleteFeature(int id) {
    var response = await featureService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }

  // Client routes
  [HttpGet("user")]
  [Authorize(Roles = "Member")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await featureService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("user/all")]
  [Authorize(Roles = "Member")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetAll() {
    var response = await featureService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("user/{id:int}")]
  [Authorize(Roles = "Member")]
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
  public async Task<IActionResult> UserGetById(int id) {
    var response = await featureService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("user")]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> UserCreateFeature([FromForm] FeatureCreateDto featureDto) {
    var response = await featureService.Create(featureDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("user/{id:int}")]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> UserUpdateFeature(int id, [FromForm] FeatureUpdateDto featureDto) {
    var response = await featureService.Update(id, featureDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpDelete("user/{id:int}")]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> UserDeleteFeature(int id) {
    var response = await featureService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }
}