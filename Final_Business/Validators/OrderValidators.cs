using Final_Business.DTOs.General;
using FluentValidation;

namespace Final_Business.Validators;

public class OrderCreateValidator : AbstractValidator<OrderCreateDto> {
  public OrderCreateValidator() {
    RuleFor(x => x.AppUserId).NotEmpty();
    RuleFor(x => x.HouseId).NotEmpty();
    RuleFor(x => x.Address).NotEmpty();
    RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
  }
}
