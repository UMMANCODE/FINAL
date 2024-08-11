using System.ComponentModel.DataAnnotations;

namespace Final_UI.Helpers.Attributes;

public class GreaterThanAttribute(string comparisonProperty) : ValidationAttribute {
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
    var currentValue = (decimal?)value;

    var property = validationContext.ObjectType.GetProperty(comparisonProperty);
    if (property == null) return new ValidationResult($"Unknown property: {comparisonProperty}");

    var comparisonValue = (decimal?)property.GetValue(validationContext.ObjectInstance);

    return currentValue < comparisonValue ? new ValidationResult($"{validationContext.DisplayName} must be greater than or equal to {comparisonProperty}") : ValidationResult.Success;
  }
}