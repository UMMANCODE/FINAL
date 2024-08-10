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
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Final_Business.Services.Implementations;
public class UserAuthService(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService) : IUserAuthService {
  public async Task<BaseResponse> Login(UserLoginDto loginDto) {
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

    return new BaseResponse(200, "Login successful!", token, []);
  }

  public async Task<BaseResponse> Register(UserRegisterDto registerDto) {
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
    return new BaseResponse(201, "User registered successfully!", user.Id, []);
  }

  public async Task<BaseResponse> ForgetPassword(UserForgetPasswordDto forgetPasswordDto) {
    if (string.IsNullOrEmpty(forgetPasswordDto.Email))
      throw new RestException(StatusCodes.Status400BadRequest, "Email is required!");
    var user = await userManager.FindByEmailAsync(forgetPasswordDto.Email)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    // Send url to user email
    var url = new Uri($"{configuration.GetSection("JWT:Audience").Value}api/Auth/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}");
    emailService.Send(user.Email!, "Password Reset", EmailTemplates.GetForgetPasswordTemplate(url.ToString()));

    return new BaseResponse(200, "Password reset link sent to your email!", token, []);
  }

  public async Task<BaseResponse> ResetPassword(UserResetPasswordDto resetPasswordDto) {
    var user = await userManager.FindByEmailAsync(resetPasswordDto.Email!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token!, resetPasswordDto.Password!);
    if (result.Succeeded) return new BaseResponse(200, "Password reset successfully!", null, []);
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new RestException(StatusCodes.Status400BadRequest, errors);
  }

  public async Task<BaseResponse> VerifyEmail(UserVerifyEmailDto verifyEmailDto) {
    var user = await userManager.FindByEmailAsync(verifyEmailDto.Email!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var result = await userManager.ConfirmEmailAsync(user, verifyEmailDto.Token!);
    if (result.Succeeded) return new BaseResponse(200, "Email verified successfully!", null, []);
    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new RestException(StatusCodes.Status400BadRequest, errors);
  }

  public async Task<BaseResponse> GetUsers() {
    var users = await userManager.Users.ToListAsync();
    var usersDto = users.Select(x => new {
      x.Id,
      x.UserName,
      x.FullName,
      x.Email,
      x.AvatarLink,
      Roles = userManager.GetRolesAsync(x).Result.ToList()
    }).ToList();
    return new BaseResponse(200, "Users fetched successfully!", usersDto, []);
  }
}
