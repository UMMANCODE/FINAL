namespace Final_Core.Entities;
public class Bid : BaseEntity {
  public decimal Amount { get; set; }
  public string? AppUserId { get; set; }
  public AppUser? AppUser { get; set; }
  public int HouseId { get; set; }
  public House? House { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;
}
