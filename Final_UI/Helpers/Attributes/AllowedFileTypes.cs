namespace Final_UI.Helpers.Attributes;

public class AllowedFileTypes(params string[] types) : ValidationAttribute {
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

    if (fileList.OfType<IFormFile?>().All(file => types.Contains(file!.ContentType))) return ValidationResult.Success;
    var errorMessage = "File must be one of the types: " + string.Join(", ", types);
    return new ValidationResult(errorMessage);
  }
}