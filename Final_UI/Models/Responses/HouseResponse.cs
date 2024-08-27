namespace Final_UI.Models.Responses;

public class HouseResponse {
  public int Id { get; set; }
  public bool IsFeatured { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public string? Location { get; set; }
  public decimal Price { get; set; }
  public string? PropertyId { get; set; }
  public float HomeArea { get; set; }
  public byte Rooms { get; set; }
  public byte Bedrooms { get; set; }
  public byte Bathrooms { get; set; }
  public int BuiltYear { get; set; }
  public List<HouseImageResponse> HouseImages { get; set; } = [];
  public PropertyStatus Status { get; set; }
  public PropertyType Type { get; set; }
  public PropertyState State { get; set; }
}
