namespace Final_Business.Validators;
public class DiscountCreateValidator : AbstractValidator<DiscountCreateDto> {
  public DiscountCreateValidator() {
    RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
    RuleFor(x => x.HouseId).NotEmpty();
    RuleFor(x => x.ExpiryDate).NotEmpty();
    RuleFor(x => x.Code).MaximumLength(50);
  }
}
