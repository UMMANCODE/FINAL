namespace Final_API.Controllers;
[Route("api/trait/[controller]")]
[ApiController]
public class HealthController : ControllerBase {
  [HttpGet]
  public IActionResult Get() {
    var response = new BaseResponse(200, "Healthy!", null, []);
    return StatusCode(response.StatusCode, response);
  }
}
