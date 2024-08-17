using Final_UI.Helpers.Enums;

namespace Final_UI.Models.Responses;

public class OrderResponse {
  public int Id { get; set; }
  public OrderStatus Status { get; set; }
  public string AppUserId { get; set; }
  public string AppUserAvatarLink { get; set; }
  public string AppUserUserName { get; set; }
  public int HouseId { get; set; }
  public decimal Price { get; set; }
  public DateTime CreatedAt { get; set; }
  public string Address { get; set; }
}
