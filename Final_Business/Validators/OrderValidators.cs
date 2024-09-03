namespace Final_Business.Validators;

public class OrderCreateValidator : AbstractValidator<OrderCreateDto> {
  public OrderCreateValidator() {
    RuleFor(x => x.HouseId).NotEmpty();
    RuleFor(x => x.Address).NotEmpty();
  }
}
