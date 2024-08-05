using Final_Business.DTOs.User;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController(
    IUserAuthService userAuthService,
    RoleManager<IdentityRole> roleManager,
    UserManager<AppUser> userManager)
    : ControllerBase {
    [HttpGet("mock")]
    [Authorize(Roles = "Developer")]
    public async Task<IActionResult> CreateUser() {

      await roleManager.CreateAsync(new IdentityRole("Admin"));
      await roleManager.CreateAsync(new IdentityRole("Member"));


      AppUser user1 = new() {
        FullName = "Admin",
        UserName = "admin",
        Email = "admin@quarter.est.com"
      };

      await userManager.CreateAsync(user1, "Admin123");

      AppUser user2 = new() {
        FullName = "Member",
        UserName = "member",
        Email = "member@quarter.est.com"
      };

      await userManager.CreateAsync(user2, "Member123");

      await userManager.AddToRoleAsync(user1, "Admin");
      await userManager.AddToRoleAsync(user2, "Member");

      return Ok("Initialization completed successfully!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRegisterDto registerDto) {
      var userId = await userAuthService.Register(registerDto);
      return Ok(new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] UserLoginDto loginDto) {
      var token = await userAuthService.Login(loginDto);
      return Ok(new { token });
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(UserForgetPasswordDto forgetPasswordDto) {
      var token = await userAuthService.ForgetPassword(forgetPasswordDto);
      return Ok(new { token });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(UserResetPasswordDto resetPasswordDto) {
      var token = await userAuthService.ResetPassword(resetPasswordDto);
      return Ok(new { token });
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] UserVerifyEmailDto verifyEmailDto) {
      var token = await userAuthService.VerifyEmail(verifyEmailDto);
      return Ok(new { token });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("community")]
    public async Task<IActionResult> GetUsers() {
      var users = await userAuthService.GetUsers();
      return Ok(users);
    }
  }
}
