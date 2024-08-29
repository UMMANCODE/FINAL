using Final_UI.Helpers.Attributes;

namespace Final_UI.Models.Requests;

public class HouseCreateRequest {
  [Required] [MaxLength(50)]
  public string? Name { get; set; }
  [Required] [MaxLength(500)]
  public string? Description { get; set; }
  [Required] [MaxLength(100)]
  public string? Location { get; set; }
  [Required] [Range(0, (double)decimal.MaxValue)]
  public decimal Price { get; set; }
  [Required]
  [Range(0, float.MaxValue)]
  public float HomeArea { get; set; }
  [Required] [Range(0, byte.MaxValue)]
  public byte Rooms { get; set; }
  [Required] [Range(0, byte.MaxValue)]
  public byte Bedrooms { get; set; }
  [Required] [Range(0, byte.MaxValue)]
  public byte Bathrooms { get; set; }
  [Required] [Range(0, int.MaxValue)]
  public int BuiltYear { get; set; }
  public bool IsFeatured { get; set; }
  public PropertyStatus Status { get; set; }
  public PropertyType Type { get; set; }
  public PropertyState State { get; set; }
  [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)] [ImageCount(0, 5)]
  public List<IFormFile> Images { get; set; } = [];
  public bool IsAdmin { get; set; } = true;
  [Required] 
  public List<int> SelectedFeatures { get; set; } = [];
}

public class HouseUpdateRequest {
  [Required]
  [MaxLength(50)]
  public string? Name { get; set; }
  [Required]
  [MaxLength(500)]
  public string? Description { get; set; }
  [Required]
  [MaxLength(100)]
  public string? Location { get; set; }
  [Required]
  [Range(0, (double)decimal.MaxValue)]
  public decimal Price { get; set; }
  [Required]
  [Range(0, float.MaxValue)]
  public float HomeArea { get; set; }
  [Required]
  [Range(0, byte.MaxValue)]
  public byte Rooms { get; set; }
  [Required]
  [Range(0, byte.MaxValue)]
  public byte Bedrooms { get; set; }
  [Required]
  [Range(0, byte.MaxValue)]
  public byte Bathrooms { get; set; }
  [Required]
  [Range(0, int.MaxValue)]
  public int BuiltYear { get; set; }
  public bool IsFeatured { get; set; }
  public PropertyStatus Status { get; set; }
  public PropertyType Type { get; set; }
  public PropertyState State { get; set; }
  [AllowedFileTypes("image/jpeg", "image/png")]
  [MaxSize(2 * 1024 * 1024)]
  [ImageCount(0, 5)]
  public List<IFormFile> Images { get; set; } = [];
  public List<int> IdsToDelete { get; set; } = [];
  public List<int> SelectedFeatures { get; set; } = [];
  public bool IsAdmin { get; set; } = true;
}
