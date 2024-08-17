using Final_UI.Models.Responses;

namespace Final_UI.Services.Interfaces;

public interface IUserService {
  public string? GetUserName();
  public string? GetRole();
  public string? GetEmail();
  public string? GetFullName();
  public Task<AppUserResponse?>? GetUsers();
}
