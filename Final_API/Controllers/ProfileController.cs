namespace Final_API.Controllers;
[Route("api/trait/[controller]")]
[ApiController]
public class ProfileController(IUserProfileService userProfileService) : ControllerBase {

  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetProfile() {
    var response = await userProfileService.GetProfile();
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("update-details")]
  [Authorize]
  public async Task<IActionResult> UpdateProfile([FromForm] UserChangeDetailsDto changeDetailsDto) {
    var response = await userProfileService.UpdateProfile(changeDetailsDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("change-password")]
  [Authorize]
  public async Task<IActionResult> ChangePassword(UserChangePasswordDto changePasswordDto) {
    var response = await userProfileService.ChangePassword(changePasswordDto);
    return StatusCode(response.StatusCode, response);
  }
}
