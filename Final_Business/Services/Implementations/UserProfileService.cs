using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Final_Business.Services.Implementations;
public class UserProfileService(UserManager<AppUser> userManager, IHttpContextAccessor accessor, IWebHostEnvironment env, IConfiguration configuration)
  : IUserProfileService {

  private readonly string _host = configuration.GetSection("HOST").Value!;

  public async Task<BaseResponse> GetProfile() {
    var token = accessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                ?? throw new RestException(StatusCodes.Status401Unauthorized, "Unauthorized");

    var user = await userManager.FindByIdAsync(JwtHelper.GetClaimFromJwt(token, ClaimTypes.NameIdentifier)!) 
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found.");

    var uriBuilder = new UriBuilder(accessor.HttpContext!.Request.Scheme, accessor.HttpContext.Request.Host.Host, accessor.HttpContext.Request.Host.Port ?? -1);
    if (uriBuilder.Uri.IsDefaultPort) uriBuilder.Port = -1;
    var baseUrl = uriBuilder.Uri.AbsoluteUri;

    var isDocker = configuration.GetValue<bool>("IsDocker");

    if (isDocker) {
      baseUrl = baseUrl.Replace("final.api", _host);
    }

    if (user.AvatarLink != null && !user.AvatarLink.Contains("google"))
      user.AvatarLink = $"{baseUrl.Replace("/api", "/")}images/users/{user.AvatarLink}";

    var profile = mapper.Map<UserProfileDto>(user);

    return new BaseResponse(200, "Success", profile, []);
  }

  public async Task<BaseResponse> UpdateProfile(UserChangeDetailsDto userChangeDetailsDto) {
    var token = accessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                ?? throw new RestException(StatusCodes.Status401Unauthorized, "Unauthorized");

    var user = await userManager.FindByIdAsync(JwtHelper.GetClaimFromJwt(token, ClaimTypes.NameIdentifier)!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");

    if (userChangeDetailsDto.UserName != null &&
        userChangeDetailsDto.UserName != user.UserName &&
        await userManager.FindByNameAsync(userChangeDetailsDto.UserName) != null) {
      throw new RestException(StatusCodes.Status400BadRequest, "Username already exists!");
    }

    user.FullName = userChangeDetailsDto.FullName ?? user.FullName;
    user.UserName = userChangeDetailsDto.UserName ?? user.UserName;

    if (userChangeDetailsDto.Avatar != null) {
      var uploadedFileName = FileManager.Save(userChangeDetailsDto.Avatar, env.WebRootPath, "images/users");

      if (user.AvatarLink != null)
        FileManager.Delete(env.WebRootPath, "images/users", user.AvatarLink);

      user.AvatarLink = uploadedFileName;
    }

    await userManager.UpdateAsync(user);

    return new BaseResponse(200, "Profile updated successfully", null, []);
  }

  public async Task<BaseResponse> ChangePassword(UserChangePasswordDto userChangePasswordDto) {
    var token = accessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                ?? throw new RestException(StatusCodes.Status401Unauthorized, "Unauthorized");

    var user = await userManager.FindByIdAsync(JwtHelper.GetClaimFromJwt(token, ClaimTypes.NameIdentifier)!)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");

    if (!await userManager.CheckPasswordAsync(user, userChangePasswordDto.OldPassword)) {
      throw new RestException(StatusCodes.Status400BadRequest, "Old password is incorrect!");
    }

    if (userChangePasswordDto.NewPassword != userChangePasswordDto.ConfirmPassword) {
      throw new RestException(StatusCodes.Status400BadRequest, "New password and confirm password do not match!");
    }

    var result = await userManager.ChangePasswordAsync(user, userChangePasswordDto.OldPassword, userChangePasswordDto.NewPassword);

    if (result.Succeeded)
      return new BaseResponse(200, "Password changed successfully", null, []);

    var errors = string.Join(", ", result.Errors.Select(x => x.Description));
    throw new RestException(StatusCodes.Status400BadRequest, errors);
  }
}
