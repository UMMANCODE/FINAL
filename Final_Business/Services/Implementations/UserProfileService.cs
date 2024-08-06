using Final_Business.DTOs.User;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Final_Business.Services.Implementations;
public class UserProfileService(UserManager<AppUser> userManager, IHttpContextAccessor accessor, IWebHostEnvironment env)
  : IUserProfileService {
  public async Task<BaseResponse> GetProfile() {
    var user = await userManager.GetUserAsync(accessor.HttpContext!.User)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");

    var uploadedFilePath = new Uri(
      $"{accessor.HttpContext!.Request.Scheme}://{accessor.HttpContext!.Request.Host}/images/users/{user.AvatarLink}"
    );

    user.AvatarLink = uploadedFilePath.ToString();
    return new BaseResponse(200, "Success", user, []);
  }

  public async Task<BaseResponse> UpdateProfile(UserChangeDetailsDto userChangeDetailsDto) {
    var user = await userManager.GetUserAsync(accessor.HttpContext!.User)
               ?? throw new RestException(StatusCodes.Status404NotFound, "User not found!");

    if (userChangeDetailsDto.Email != user.Email && userChangeDetailsDto.Email is not null) {
      var emailExists = await userManager.FindByEmailAsync(userChangeDetailsDto.Email);
      if (emailExists != null) {
        throw new RestException(StatusCodes.Status400BadRequest, "Email already exists!");
      }
    }

    user.Email = userChangeDetailsDto.Email ?? user.Email;
    user.FullName = userChangeDetailsDto.FullName ?? user.FullName;
    user.UserName = userChangeDetailsDto.UserName ?? user.UserName;

    if (userChangeDetailsDto.Avatar != null) {
      var uploadedFileName = FileManager.Save(userChangeDetailsDto.Avatar, env.WebRootPath, "images/users");

      if (user.AvatarLink != null)
        FileManager.Delete(env.WebRootPath, "images/users", user.AvatarLink);

      user.AvatarLink = uploadedFileName;
    }

    await userManager.UpdateAsync(user);

    return new BaseResponse(204, "Profile updated successfully", null, []);
  }

  public async Task<BaseResponse> ChangePassword(UserChangePasswordDto userChangePasswordDto) {
    var user = await userManager.GetUserAsync(accessor.HttpContext!.User)
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
