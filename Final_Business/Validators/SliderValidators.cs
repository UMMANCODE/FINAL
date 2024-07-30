using Final_Business.DTOs.General;
using FluentValidation;

namespace Final_Business.Validators;
public class SliderCreateValidator : AbstractValidator<SliderCreateDto> {
  public SliderCreateValidator() {
    RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
    RuleFor(x => x.SubTitle1).NotEmpty().MaximumLength(100);
    RuleFor(x => x.SubTitle2).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.BtnText1).MaximumLength(20);
    RuleFor(x => x.BtnText2).MaximumLength(20);
    RuleFor(x => x.Image).NotEmpty();
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
    RuleFor(x => x.Image).NotEmpty();
  }
}