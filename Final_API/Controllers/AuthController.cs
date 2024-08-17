using Final_Business.DTOs.User;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
  IUserAuthService userAuthService,
  RoleManager<IdentityRole> roleManager,
  UserManager<AppUser> userManager,
  IConfiguration configuration)
  : ControllerBase {
  [HttpGet("mock")]
  [Authorize(Roles = "Developer")]
  public async Task<IActionResult> CreateUser() {

    await roleManager.CreateAsync(new IdentityRole("Admin"));
    await roleManager.CreateAsync(new IdentityRole("Member"));


    AppUser user1 = new() {
      FullName = "Admin",
      UserName = "admin",
      Email = "admin@quarter.est.com",
      EmailConfirmed = true
    };

    await userManager.CreateAsync(user1, "Admin123");

    AppUser user2 = new() {
      FullName = "Member",
      UserName = "member",
      Email = "member@quarter.est.com",
      EmailConfirmed = true
    };

    await userManager.CreateAsync(user2, "Member123");

    await userManager.AddToRoleAsync(user1, "Admin");
    await userManager.AddToRoleAsync(user2, "Member");

    return Ok("Initialization completed successfully!");
  }

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

  [HttpPost("manage-login")]
  public async Task<IActionResult> AdminLogin(UserLoginDto loginDto) {
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
    var props = new AuthenticationProperties { RedirectUri = "api/auth/signin-google" };
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
    var locale = response.Principal.FindFirstValue("locale");


    if (email == null || fullName == null || userName == null) return BadRequest();

    BaseResponse result = new(400, "Something went wrong!", null, null);

    if (userManager.Users.Any(x => x.Email == email)) {
      var user = new UserLoginDto(email, "", false, true);
      result = await userAuthService.Login(user);
    }
    else {
      var guidPassword = new Guid();
      var password = guidPassword.ToString()[..8];
      var registerDto = new UserRegisterDto(email, fullName, userName, password, password, null, null);
      var registerResult = await userAuthService.Register(registerDto);

      if (registerResult.StatusCode == 201) {
        var user = new UserLoginDto(email, password, false, true);
        result = await userAuthService.Login(user);
      }
    }

    var redirectUrl = $"{configuration.GetSection("Client:URL").Value}Account/ExternalLoginCallback?token={result.Data}";
    return Redirect(redirectUrl);
  }

  [Authorize(Roles = "Admin, SuperAdmin")]
  [HttpGet("community")]
  public async Task<IActionResult> GetUsers() {
    var response = await userAuthService.GetUsers();
    return StatusCode(response.StatusCode, response);
  }
}