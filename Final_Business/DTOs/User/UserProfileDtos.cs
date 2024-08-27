namespace Final_Business.DTOs.User;

public record UserChangeDetailsDto(
     string? FullName, string? UserName, IFormFile? Avatar
);

public record UserChangePasswordDto(
     string OldPassword, string NewPassword, string ConfirmPassword
);
