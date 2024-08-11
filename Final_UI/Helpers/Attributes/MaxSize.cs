using System.ComponentModel.DataAnnotations;

namespace Final_UI.Helpers.Attributes;

public class MaxSize(int byteSize) : ValidationAttribute {
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
    List<IFormFile> fileList = [];

    switch (value) {
      case null:
        return ValidationResult.Success;
      case List<IFormFile> files:
        fileList = files;
        break;
      case IFormFile file:
        fileList.Add(file);
        break;
    }

    if (!fileList.OfType<IFormFile?>().Any(file => file!.Length > byteSize)) return ValidationResult.Success;
    var mb = byteSize / 1024d / 1024d;
    return new ValidationResult($"File must be less than or equal to {mb:0.##}mb");
  }
}