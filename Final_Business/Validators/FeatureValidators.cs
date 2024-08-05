using Final_Business.DTOs.General;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Final_Business.Validators;

public class CreateFeatureValidator : AbstractValidator<FeatureCreateDto> {
  public CreateFeatureValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Icon)
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

public class UpdateFeatureValidator : AbstractValidator<FeatureUpdateDto> {
  public UpdateFeatureValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Icon)
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
