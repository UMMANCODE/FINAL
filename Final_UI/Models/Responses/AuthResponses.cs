namespace Final_UI.Models.Responses;

public class LoginResponse {
  public string? Token { get; set; }
}

public class RegisterResponse {
  public string? UserId { get; set; }
}

public class CreateAdminResponse {
  public string? UserId { get; set; }
}

public class ForgetPasswordResponse {
  public string? Token { get; set; }
}
