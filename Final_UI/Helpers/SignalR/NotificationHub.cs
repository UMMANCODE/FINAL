using Microsoft.AspNetCore.SignalR;

namespace Final_UI.Helpers.SignalR;

public class NotificationHub : Hub {
  public async Task SendNotification(string userId, string message) {
    await Clients.User(userId).SendAsync("notify", message);
  }
}
