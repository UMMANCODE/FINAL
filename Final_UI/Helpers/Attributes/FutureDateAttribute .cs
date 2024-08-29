namespace Final_UI.Helpers.Attributes;

public class FutureDateAttribute : ValidationAttribute {
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
    if (value is not DateTime dateTime) return new ValidationResult("Invalid date.");
    return dateTime > DateTime.Now ? ValidationResult.Success : new ValidationResult("Date must be in the future.");
  }
}