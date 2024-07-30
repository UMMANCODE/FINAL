using Final_Business.DTOs.General;
using FluentValidation;

namespace Final_Business.Validators;

public class CreateFeatureValidator : AbstractValidator<FeatureCreateDto> {
  public CreateFeatureValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Icon).NotEmpty();
  }
}

public class UpdateFeatureValidator : AbstractValidator<FeatureUpdateDto> {
  public UpdateFeatureValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Icon).NotEmpty();
  }
}
