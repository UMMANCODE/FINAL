using Final_Business.DTOs.User;
using FluentValidation;

namespace Final_Business.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDto> {
  public UserLoginValidator() {
    RuleFor(x => x.UserName).NotEmpty();
    RuleFor(x => x.Password).NotEmpty();
  }
}

public class UserRegisterValidator : AbstractValidator<UserRegisterDto> {
  public UserRegisterValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    RuleFor(x => x.Password).NotEmpty();
    RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
    RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
    RuleFor(x=> x.UserName).NotEmpty().MaximumLength(50);
  }
}

public class UserForgetPasswordValidator : AbstractValidator<UserForgetPasswordDto> {
  public UserForgetPasswordValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
  }
}

public class UserResetPasswordValidator : AbstractValidator<UserResetPasswordDto> {
  public UserResetPasswordValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Password).NotEmpty();
    RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
  }
}

public class UserVerifyEmailValidator : AbstractValidator<UserVerifyEmailDto> {
  public UserVerifyEmailValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Token).NotEmpty();
  }
}

public class UserSendVerifyEmailValidator : AbstractValidator<UserSendVerifyEmailDto> {
  public UserSendVerifyEmailValidator() {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
  }
}