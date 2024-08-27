namespace Final_UI.Services.Interfaces;

public interface IUserService {
  public string? GetUserId();
  public string? GetUserName();
  public string? GetRole();
  public string? GetEmail();
  public string? GetFullName();
  public Task<List<AppUserResponse>?> GetUsers();
}
