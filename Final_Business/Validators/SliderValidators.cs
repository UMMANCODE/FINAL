namespace Final_Business.Validators;
public class SliderCreateValidator : AbstractValidator<SliderCreateDto> {
  public SliderCreateValidator() {
    RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
    RuleFor(x => x.SubTitle1).NotEmpty().MaximumLength(100);
    RuleFor(x => x.SubTitle2).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.BtnText1).MaximumLength(20);
    RuleFor(x => x.BtnText2).MaximumLength(20);
    RuleFor(x => x.Order).GreaterThanOrEqualTo(1);
    RuleFor(x => x.Image)
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

public class SliderUpdateValidator : AbstractValidator<SliderUpdateDto> {
  public SliderUpdateValidator() {
    RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
    RuleFor(x => x.SubTitle1).NotEmpty().MaximumLength(100);
    RuleFor(x => x.SubTitle2).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.BtnText1).MaximumLength(20);
    RuleFor(x => x.BtnText2).MaximumLength(20);
    RuleFor(x => x.Order).GreaterThanOrEqualTo(1);
    RuleFor(x => x.Image)
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