namespace Final_Business.Validators;
public class UserBidValidators : AbstractValidator<UserBidCreateDto> {
  public UserBidValidators() {
    RuleFor(x => x.HouseId).NotEmpty();
    RuleFor(x => x.Amount).NotEmpty();
  }
}
