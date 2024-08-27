namespace Final_Core.Entities;
public class Comment : BaseEntity {
  public int HouseId { get; set; }
  public House House { get; set; }
  public string AppUserId { get; set; }
  public AppUser AppUser { get; set; }
  public string Content { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public CommentStatus Status { get; set; } = CommentStatus.Pending;
}
