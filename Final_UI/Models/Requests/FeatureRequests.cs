using Final_UI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Final_UI.Models.Requests;

public class FeatureCreateRequest {
  [Required] [MaxLength(50)]
  public string? Name { get; set; }
  [Required] [MaxLength(500)]
  public string? Description { get; set; }
  [Required] [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)]
  public IFormFile? Icon { get; set; }
}

public class FeatureUpdateRequest {
  [Required] [MaxLength(50)] 
  public string? Name { get; set; }
  [Required] [MaxLength(500)] 
  public string? Description { get; set; }

  [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)]
  public IFormFile? Icon { get; set; }
}
