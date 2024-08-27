using Microsoft.AspNetCore.SignalR;

namespace Final_UI.Helpers.SignalR;

public class CustomUserIdProvider(IServiceProvider serviceProvider) : IUserIdProvider {
  public string? GetUserId(HubConnectionContext connection) {
    using var scope = serviceProvider.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    var user = userService.GetUserId();

    return user;
  }
}
