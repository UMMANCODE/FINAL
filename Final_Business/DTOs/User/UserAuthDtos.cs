namespace Final_Business.DTOs.User;

public record UserLoginDto(
  string Email, string Password
);

public record UserRegisterDto(
   string Email, string FullName, string UserName, string Password, string ConfirmPassword
);

public record UserVerifyEmailDto(
  string Email, string Token
);

public record UserForgotPasswordDto(
   string Email
);

public record UserResetPasswordDto(
    string Email, string Token, string Password, string ConfirmPassword
);
