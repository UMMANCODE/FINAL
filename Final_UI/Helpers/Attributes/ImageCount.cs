using System.ComponentModel.DataAnnotations;

namespace Final_UI.Helpers.Attributes;

public class ImageCountAttribute(int min, int max) : ValidationAttribute {
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
    if (value is not List<IFormFile> images) return ValidationResult.Success;

    if (images.Count < min) return new ValidationResult($"The {validationContext.DisplayName} field must have at least {min} image(s).");

    return images.Count > max ? new ValidationResult($"The {validationContext.DisplayName} field must not have more than {max} images.") : ValidationResult.Success;
  }
}