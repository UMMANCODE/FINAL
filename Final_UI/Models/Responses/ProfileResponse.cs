namespace Final_UI.Models.Responses;

public class ProfileResponse {
  public ChangeDetailsRequest ChangeDetailsRequest { get; set; } = new();
  public ChangePasswordRequest ChangePasswordRequest { get; set; } = new();
}
