namespace Final_Core.Entities;
public class Bid {
  public int Id { get; set; }
  public decimal Amount { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public string? UserId { get; set; }
  public AppUser? User { get; set; }
  public int HouseId { get; set; }
  public House? House { get; set; }
}
