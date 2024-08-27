using Final_UI.Helpers.Attributes;

namespace Final_UI.Models.Requests;

public class ChangePasswordRequest {
  [Required]
  [DataType(DataType.Password)]
  public string? OldPassword { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? NewPassword { get; set; }
  [Required]
  [DataType(DataType.Password)]
  [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
  public string? ConfirmPassword { get; set; }
}

public class ChangeDetailsRequest {
  [Required]
  public string? FullName { get; set; }
  [Required]
  public string? UserName { get; set; }
  [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)]
  public IFormFile? Avatar { get; set; }
}
