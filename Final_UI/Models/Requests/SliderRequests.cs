using Final_UI.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;
using Final_UI.Helpers.Enums;

namespace Final_UI.Models.Requests;

public class SliderCreateRequest {
  [Required] [MaxLength(50)]
  public string? Title { get; set; }
  [Required] [MaxLength(100)]
  public string? SubTitle1 { get; set; }
  [Required] [MaxLength(100)]
  public string? SubTitle2 { get; set; }
  [Required] [MaxLength(500)]
  public string? Description { get; set; }
  [Required] [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)]
  public IFormFile? Image { get; set; }
  [MaxLength(20)]
  public string? BtnText1 { get; set; }
  [MaxLength(20)]
  public string? BtnText2 { get; set; }
  public string? BtnLink1 { get; set; }
  public string? BtnLink2 { get; set; }
  public ImagePosition ImagePosition { get; set; }
  public int Order { get; set; }
}

public class SliderUpdateRequest {
  [Required] [MaxLength(50)]
  public string? Title { get; set; }
  [Required] [MaxLength(100)]
  public string? SubTitle1 { get; set; }
  [Required] [MaxLength(100)]
  public string? SubTitle2 { get; set; }
  [Required] [MaxLength(500)]
  public string? Description { get; set; }
  [AllowedFileTypes("image/jpeg", "image/png")] [MaxSize(2 * 1024 * 1024)]
  public IFormFile? Image { get; set; }
  [MaxLength(20)]
  public string? BtnText1 { get; set; }
  [MaxLength(20)]
  public string? BtnText2 { get; set; }
  public string? BtnLink1 { get; set; }
  public string? BtnLink2 { get; set; }
  public ImagePosition ImagePosition { get; set; }
  public int Order { get; set; }
}
