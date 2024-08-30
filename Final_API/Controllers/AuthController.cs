using Final_Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Final_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
  IUserAuthService userAuthService,
  UserManager<AppUser> userManager,
  IConfiguration configuration)
  : ControllerBase {

  private readonly string _host = configuration.GetSection("HOST").Value!;
  private readonly string _port = configuration.GetSection("PORT").Value!;
  private readonly string _protocol = configuration.GetSection("PROTOCOL").Value!;

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromForm] UserRegisterDto registerDto) {
    var response = await userAuthService.Register(registerDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(UserLoginDto loginDto) {
    var response = await userAuthService.Login(loginDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("forget-password")]
  public async Task<IActionResult> ForgetPassword(UserForgetPasswordDto forgetPasswordDto) {
    var response = await userAuthService.ForgetPassword(forgetPasswordDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword(UserResetPasswordDto resetPasswordDto) {
    var response = await userAuthService.ResetPassword(resetPasswordDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("verify-email")]
  public async Task<IActionResult> VerifyEmail(UserVerifyEmailDto verifyEmailDto) {
    var response = await userAuthService.VerifyEmail(verifyEmailDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost("send-verify-email")]
  public async Task<IActionResult> SendVerifyEmail(UserSendVerifyEmailDto sendVerifyEmailDto) {
    var response = await userAuthService.SendVerifyEmail(sendVerifyEmailDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("login-google")]
  public IActionResult Login() {
    var props = new AuthenticationProperties { RedirectUri = $"{_protocol}://{_host}:{_port}/api/auth/signin-google" };
    return Challenge(props, GoogleDefaults.AuthenticationScheme);
  }

  [HttpGet("signin-google")]
  public async Task<IActionResult> GoogleLogin() {
    var response = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
    if (response.Principal == null) return BadRequest();

    var email = response.Principal.FindFirstValue(ClaimTypes.Email);
    var fullName = response.Principal.FindFirstValue(ClaimTypes.Name);
    var userName = response.Principal.FindFirstValue(ClaimTypes.Email);
    var profilePicture = response.Principal.FindFirstValue("picture");
    var location = await GetUserLocationAsync() ?? "N/A";


    if (email == null || fullName == null || userName == null) return BadRequest();

    BaseResponse result = new(400, "Something went wrong!", null, null);

    if (userManager.Users.Any(x => x.Email == email)) {
      var user = new UserLoginDto(email, "", false, true);
      result = await userAuthService.Login(user);
    }
    else {
      var guidPassword = new Guid();
      var password = guidPassword.ToString()[..8];
      var registerDto = new UserRegisterDto(email, fullName, userName, password, password, null, location, true);
      var registerResult = await userAuthService.Register(registerDto);

      if (registerResult.StatusCode == 201) {
        var user = new UserLoginDto(email, password, false, true);
        result = await userAuthService.Login(user);
      }
    }

    var userEntity = await userManager.FindByEmailAsync(email);
    userEntity!.AvatarLink = profilePicture;
    await userManager.UpdateAsync(userEntity);
    
    var clientUrl = configuration.GetSection("Client:URL").Value!;

    if (clientUrl.Contains("final.ui")) {
      clientUrl = clientUrl.Replace("final.ui", _host);
    }

    var redirectUrl = $"{clientUrl}Account/ExternalLoginCallback?token={result.Data}";
    return Redirect(redirectUrl);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("community")]
  public async Task<IActionResult> GetUsers() {
    var response = await userAuthService.GetUsers();
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "SuperAdmin")]
  [HttpPost("create-super")]
  public async Task<IActionResult> CreateAdmin([FromForm] UserCreateAdminDto createAdminDto) {
    var response = await userAuthService.CreateAdmin(createAdminDto);
    return StatusCode(response.StatusCode, response);
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePassword(UserForceChangePasswordDto changePasswordDto) {
    var response = await userAuthService.ChangePassword(changePasswordDto);
    return StatusCode(response.StatusCode, response);
  }

  private static async Task<string?> GetUserLocationAsync() {
    using var httpClient = new HttpClient();
    var ipInfo = await httpClient.GetStringAsync("https://ipinfo.io");
    dynamic ipData = Newtonsoft.Json.JsonConvert.DeserializeObject(ipInfo);
    return ipData?.country;
  }
}