using Final_Core.Enums;

namespace Final_Core.Entities;
public class House : AuditEntity {
  public string? Name { get; set; }
  public string? Location { get; set; }
  public string? Description { get; set; }
  public decimal Price { get; set; }
  public string? PropertyId { get; set; } = Guid.NewGuid().ToString()[..8];
  public float HomeArea { get; set; }
  public byte Rooms { get; set; }
  public byte Bedrooms { get; set; }
  public byte Bathrooms { get; set; }
  public int BuiltYear { get; set; }
  public List<HouseImage> Images { get; set; } = [];
  public List<Comment> Comments { get; set; } = [];
  public PropertyStatus Status { get; set; }
  public PropertyType Type { get; set; }
  public PropertyState State { get; set; }
  public bool IsFeatured { get; set; }
  public string? OwnerId { get; set; }
  public AppUser? Owner { get; set; }
}
