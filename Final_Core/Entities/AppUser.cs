using Microsoft.AspNetCore.Identity;

namespace Final_Core.Entities;

public class AppUser : IdentityUser {
  public string? FullName { get; set; }
  public List<Comment> Comments { get; set; } = [];
  public List<House> Houses { get; set; } = [];
  public List<Bid> Bids { get; set; } = [];
  public string? AvatarLink { get; set; }
}