namespace Final_UI.Models.Requests;

public class LoginRequest {
  [Required]
  public string? UserName { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
  public bool RememberMe { get; set; }
  public bool ExternalLogin { get; set; }
}

public class RegisterRequest {
  [Required]
  public string? Email { get; set; }
  [Required]
  public string? FullName { get; set; }
  [Required]
  public string? UserName { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
  [Required]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
  public string? ConfirmPassword { get; set; }
  public IFormFile? Avatar { get; set; }
  [Required]
  public string? Nationality { get; set; }
}

public class VerifyEmailRequest {
  [Required]
  public string? Email { get; set; }
  [Required]
  public string? Token { get; set; }
}

public class SendVerifyEmailRequest {
  [Required]
  public string? Email { get; set; }
}

public class ForgetPasswordRequest {
  [Required]
  public string? Email { get; set; }
}

public class ResetPasswordRequest {
  [Required]
  public string? Email { get; set; }
  [Required] 
  public string? Token { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
  [Required]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
  public string? ConfirmPassword { get; set; }
}

public class CreateAdminRequest {
  [Required]
  public string? Email { get; set; }
  [Required]
  public string? FullName { get; set; }
  [Required]
  public string? UserName { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? Password { get; set; }
  [Required]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
  public string? ConfirmPassword { get; set; }
  public IFormFile? Avatar { get; set; }
  [Required]
  public string? Nationality { get; set; }
}

public class ForceChangePasswordRequest {
  [Required]
  [DataType(DataType.Password)]
  public string? OldPassword { get; set; }
  [Required]
  [DataType(DataType.Password)]
  public string? NewPassword { get; set; }
  [Required]
  [DataType(DataType.Password)]
  [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password must match")]
  public string? ConfirmPassword { get; set; }
}
