namespace Final_UI.Models.Responses;

public class AppUserResponse {
  public string Id { get; set; }
  public string UserName { get; set; }
  public string FullName { get; set; }
  public string AvatarLink { get; set; }
  public string Email { get; set; }
  public string Nationality { get; set; }
  public List<string> Roles { get; set; } = [];
}
