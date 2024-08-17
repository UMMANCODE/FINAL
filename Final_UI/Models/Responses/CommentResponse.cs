using Final_UI.Helpers.Enums;

namespace Final_UI.Models.Responses;

public class CommentResponse {
  public int Id { get; set; }
  public string Content { get; set; }
  public string AppUserId { get; set; }
  public int HouseId { get; set; }
  public DateTime CreatedAt { get; set; }
  public string AppUserAvatarLink { get; set; }
  public string AppUserUserName { get; set; }
  public CommentStatus Status { get; set; }
}
