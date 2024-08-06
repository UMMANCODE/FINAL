using Final_Business.DTOs.User;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Final_Business.Validators;

public class UserChangeDetailsValidator : AbstractValidator<UserChangeDetailsDto> {
  public UserChangeDetailsValidator() {
    RuleFor(x => x.Email).EmailAddress().MaximumLength(50);
    RuleFor(x => x.FullName).MaximumLength(100);
    RuleFor(x => x.UserName).MaximumLength(50);
    RuleFor(x => x.Avatar)
      .Must(BeAValidImage).WithMessage("Only JPEG and PNG images are allowed")
      .Must(BeAValidImageSize).WithMessage("Image must be less than or equal to 2MB");
  }

  private static bool BeAValidImage(IFormFile? image) {
    if (image == null) {
      return true;
    }
    return image?.ContentType is "image/jpeg" or "image/png";
  }

  private static bool BeAValidImageSize(IFormFile? image) {
    if (image == null) {
      return true;
    }
    return image?.Length <= 2 * 1024 * 1024;
  }
}

public class UserChangePasswordValidator : AbstractValidator<UserChangePasswordDto> {
  public UserChangePasswordValidator() {
    RuleFor(x => x.OldPassword).NotEmpty();
    RuleFor(x => x.NewPassword).NotEmpty();
    RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword);
  }
}

