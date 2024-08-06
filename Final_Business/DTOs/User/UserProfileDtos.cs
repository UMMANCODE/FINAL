using Microsoft.AspNetCore.Http;

namespace Final_Business.DTOs.User;

public record UserChangeDetailsDto(
     string? Email, string? FullName, string? UserName, IFormFile? Avatar
);

public record UserChangePasswordDto(
     string OldPassword, string NewPassword, string ConfirmPassword
);
