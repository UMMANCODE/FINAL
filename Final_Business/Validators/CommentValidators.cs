namespace Final_Business.Validators;

public class CommentCreateValidator : AbstractValidator<CommentCreateDto> {
  public CommentCreateValidator() {
    RuleFor(x => x.Content).NotEmpty().MaximumLength(200);
    RuleFor(x => x.HouseId).NotEmpty();
  }
}
