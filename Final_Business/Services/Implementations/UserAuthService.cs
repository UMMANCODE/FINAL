using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Final_Business.DTOs.User;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Final_Business.Services.Implementations;
public class UserAuthService(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService) : IUserAuthService {
  public async Task<string> Login(UserLoginDto loginDto) {
    var user = await userManager.FindByNameAsync(loginDto.UserName!);

    if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password!))
      throw new RestException(StatusCodes.Status401Unauthorized, "UserName or Password incorrect!");

    List<Claim> claims = [
      new Claim(ClaimTypes.NameIdentifier, user.Id),
      new Claim(ClaimTypes.Name, user.UserName!),
      new Claim("FullName", user.FullName!)
    ];

    var roles = await userManager.GetRolesAsync(user);

    claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());

    var secret = configuration.GetSection("JWT:Secret").Value!;

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

    JwtSecurityToken securityToken = new(
      issuer: configuration.GetSection("JWT:Issuer").Value,
      audience: configuration.GetSection("JWT:Audience").Value,
      claims: claims,
      signingCredentials: credentials,
      expires: DateTime.Now.AddDays(3)
    );

    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

    return token;
  }

  public async Task<string> Register(UserRegisterDto registerDto) {
    var user = new AppUser {
      UserName = registerDto.UserName,
      FullName = registerDto.FullName,
      Email = registerDto.Email
    };

    var result = await userManager.CreateAsync(user, registerDto.Password);

    if (!result.Succeeded) {
      var errors = string.Join(", ", result.Errors.Select(x => x.Description));
      throw new RestException(StatusCodes.Status400BadRequest, errors);
    }

    await userManager.AddToRoleAsync(user, "Member");
    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
    // Send url to user email
    var url = new Uri($"{configuration.GetSection("JWT:Audience").Value}api/Auth/verify-email?email={user.Email}&token={WebUtility.UrlEncode(token)}");
    emailService.Send(user.Email, "Email Verification", EmailTemplates.GetVerifyEmailTemplate(url.ToString()));
    return "Please verify your email!";
  }

  public async Task<string> ForgetPassword(UserForgetPasswordDto forgetPasswordDto) {
    if (string.IsNullOrEmpty(forgetPasswordDto.Email))
      throw new RestException(StatusCodes.Status400BadRequest, "Email is required!");
    var user = await userManager.FindByEmailAsync(forgetPasswordDto.Email)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    // Send url to user email
    var url = new Uri($"{configuration.GetSection("JWT:Audience").Value}api/Auth/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}");
    emailService.Send(user.Email!, "Password Reset", EmailTemplates.GetForgetPasswordTemplate(url.ToString()));
    return "Email sent successfully!";
  }

  public async Task<string> ResetPassword(UserResetPasswordDto resetPasswordDto) {
    var user = await userManager.FindByEmailAsync(resetPasswordDto.Email!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token!, resetPasswordDto.Password!);
    if (result.Succeeded) return JsonSerializer.Serialize(new { message = "Password reset successfully!" });
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new RestException(StatusCodes.Status400BadRequest, errors);
  }

  public async Task<string> VerifyEmail(UserVerifyEmailDto verifyEmailDto) {
    var user = await userManager.FindByEmailAsync(verifyEmailDto.Email!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var result = await userManager.ConfirmEmailAsync(user, verifyEmailDto.Token!);
    if (result.Succeeded) return JsonSerializer.Serialize(new { message = "Email verified successfully!" });
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new RestException(StatusCodes.Status400BadRequest, errors);
  }

  public async Task<List<AppUser>> GetUsers() {
    return await userManager.Users.ToListAsync();
  }
}
