namespace Final_Business.DTOs.User;

public record UserLoginDto(
  string UserName, string? Password, bool RememberMe, bool ExternalLogin
);

public record UserRegisterDto(
   string Email, string FullName, string UserName, string Password, string ConfirmPassword, IFormFile? Avatar, string Nationality, bool ExternalLogin
);

public record UserVerifyEmailDto(
  string Email, string Token
);

public record UserSendVerifyEmailDto(
  string Email
);

public record UserForgetPasswordDto(
   string Email
);

public record UserResetPasswordDto(
    string Email, string Token, string Password, string ConfirmPassword
);

public record UserCreateAdminDto(
  string Email,
  string FullName,
  string UserName,
  string Password,
  string ConfirmPassword,
  IFormFile? Avatar,
  string Nationality
);

public record UserForceChangePasswordDto(
  string OldPassword, string NewPassword, string ConfirmPassword
);
