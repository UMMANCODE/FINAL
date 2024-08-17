﻿using Final_Business.DTOs.User;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Final_Business.Services.Implementations;
public class UserAuthService(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService, IWebHostEnvironment env, IMapper mapper)
  : IUserAuthService {
  public async Task<BaseResponse> Login(UserLoginDto loginDto) {
    var user = loginDto.ExternalLogin ? await userManager.FindByEmailAsync(loginDto.UserName) : await userManager.FindByNameAsync(loginDto.UserName);

    if (!loginDto.ExternalLogin) {

      if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password!))
        throw new RestException(StatusCodes.Status401Unauthorized, "UserName or Password incorrect!");

      if (!await userManager.IsEmailConfirmedAsync(user))
        throw new RestException(StatusCodes.Status401Unauthorized, $"Email not verified!{user.Email}");
    }
    else {
      user!.EmailConfirmed = true;
      await userManager.UpdateAsync(user);
    }

    List<Claim> claims = [
      new Claim(ClaimTypes.NameIdentifier, user!.Id),
      new Claim(ClaimTypes.Name, user.UserName!),
      new Claim("FullName", user.FullName!),
      new Claim("ExternalLogin", loginDto.ExternalLogin ? "true" : "false")
    ];

    var roles = await userManager.GetRolesAsync(user);

    claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());

    var secret = configuration.GetSection("JWT:Secret").Value!;

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

    var expires = loginDto.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(1);

    JwtSecurityToken securityToken = new(
      issuer: configuration.GetSection("JWT:Issuer").Value,
      audience: configuration.GetSection("JWT:Audience").Value,
      claims: claims,
      signingCredentials: credentials,
      expires: expires
    );

    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

    return new BaseResponse(200, "Login successful!", token, []);
  }

  public async Task<BaseResponse> Register(UserRegisterDto registerDto) {
    string? uploadedFilePath = null;
    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    try
    {
      var user = mapper.Map<AppUser>(registerDto);

      if (registerDto.Avatar != null)
        uploadedFilePath = FileManager.Save(registerDto.Avatar, env.WebRootPath, "images/users");

      user.AvatarLink = uploadedFilePath;

      var result = await userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded) {
        var errors = string.Join(", ", result.Errors.Select(x => x.Description));
        throw new RestException(StatusCodes.Status400BadRequest, errors);
      }

      await userManager.AddToRoleAsync(user, "Admin");
      var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
      // Send url to user email
      var url = new Uri($"{configuration.GetSection("Client:URL").Value}Account/VerifyEmail?email={user.Email}&token={WebUtility.UrlEncode(token)}");
      emailService.Send(user.Email!, "Email Verification", EmailTemplates.GetVerifyEmailTemplate(url.ToString()));

      scope.Complete();

      return new BaseResponse(201, "User registered successfully!", new { user.Id }, []);
    }
    catch {

      if (!string.IsNullOrEmpty(uploadedFilePath)) {
        FileManager.Delete(env.WebRootPath, "images/users", uploadedFilePath);
      }

      throw;
    }
  }

  public async Task<BaseResponse> ForgetPassword(UserForgetPasswordDto forgetPasswordDto) {
    if (string.IsNullOrEmpty(forgetPasswordDto.Email))
      throw new RestException(StatusCodes.Status400BadRequest, "Email is required!");
    var user = await userManager.FindByEmailAsync(forgetPasswordDto.Email)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");
    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    // Send url to user email
    var url = new Uri($"{configuration.GetSection("Client:URL").Value}Account/ResetPassword?email={user.Email}&token={WebUtility.UrlEncode(token)}");
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

  public async Task<BaseResponse> SendVerifyEmail(UserSendVerifyEmailDto sendVerifyEmailDto) {
    var user = await userManager.FindByEmailAsync(sendVerifyEmailDto.Email)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");

    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
    // Send url to user email
    var url = new Uri($"{configuration.GetSection("Client:URL").Value}Account/VerifyEmail?email={user.Email}&token={WebUtility.UrlEncode(token)}");
    emailService.Send(user.Email!, "Email Verification", EmailTemplates.GetVerifyEmailTemplate(url.ToString()));

    return new BaseResponse(200, "Verification email sent successfully!", token, []);
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
      x.Nationality,
      Roles = userManager.GetRolesAsync(x).Result.ToList()
    }).ToList();
    return new BaseResponse(200, "Users fetched successfully!", usersDto, []);
  }
}
