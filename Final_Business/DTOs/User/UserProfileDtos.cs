using Microsoft.AspNetCore.Http;

namespace Final_Business.DTOs.User;

public record UserChangeDetailsDto(
     string Id, string Email, string FullName, string UserName, IFormFile Avatar
);

public record UserChangePasswordDto(
     string Id, string OldPassword, string NewPassword, string ConfirmPassword
);
