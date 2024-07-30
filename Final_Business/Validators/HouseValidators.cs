using Final_Business.DTOs.Admin;
using Final_Business.DTOs.User;
using FluentValidation;

namespace Final_Business.Validators;

public class AdminHouseCreateValidator : AbstractValidator<AdminHouseCreateDto> {
  public AdminHouseCreateValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
    RuleFor(x => x.HomeArea).GreaterThanOrEqualTo(0);
    RuleFor(x => x.Rooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bathrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.BuiltYear).LessThanOrEqualTo(x => DateTime.Now.Year);
    RuleFor(x => x.Status).NotEmpty();
    RuleFor(x => x.Type).NotEmpty();
    RuleFor(x => x.State).NotEmpty();
  }
}

public class AdminHouseUpdateValidator : AbstractValidator<AdminHouseUpdateDto> {
  public AdminHouseUpdateValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
    RuleFor(x => x.HomeArea).GreaterThanOrEqualTo(0);
    RuleFor(x => x.Rooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bathrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.BuiltYear).LessThanOrEqualTo(x => DateTime.Now.Year);
    RuleFor(x => x.Status).NotEmpty();
    RuleFor(x => x.Type).NotEmpty();
    RuleFor(x => x.State).NotEmpty();
  }
}

public class UserHouseCreateValidator : AbstractValidator<UserHouseUpdateDto> {
  public UserHouseCreateValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
    RuleFor(x => x.HomeArea).GreaterThanOrEqualTo(0);
    RuleFor(x => x.Rooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bathrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.BuiltYear).LessThanOrEqualTo(x => DateTime.Now.Year);
    RuleFor(x => x.Status).NotEmpty();
    RuleFor(x => x.Type).NotEmpty();
    RuleFor(x => x.State).NotEmpty();
  }
}

public class UserHouseUpdateValidator : AbstractValidator<UserHouseUpdateDto> {
  public UserHouseUpdateValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Location).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
    RuleFor(x => x.HomeArea).GreaterThanOrEqualTo(0);
    RuleFor(x => x.Rooms).GreaterThanOrEqualTo((byte)0)
      .Must((dto, rooms) => BeValidRooms(rooms, dto.Bedrooms, dto.Bathrooms))
      .WithMessage("Rooms count is not valid");
    RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.Bathrooms).GreaterThanOrEqualTo((byte)0);
    RuleFor(x => x.BuiltYear).LessThanOrEqualTo(x => DateTime.Now.Year);
    RuleFor(x => x.Status).NotEmpty();
    RuleFor(x => x.Type).NotEmpty();
    RuleFor(x => x.State).NotEmpty();
  }

  private static bool BeValidRooms(byte rooms, byte bedrooms, byte bathrooms) {
    return rooms >= bedrooms + bathrooms;
  }
}