namespace Final_Core.Entities;

public class Slider : AuditEntity {
  public string? Title { get; set; }
  public string? SubTitle1 { get; set; }
  public string? SubTitle2 { get; set; }
  public string? Description { get; set; }
  public string? ImageLink { get; set; }
  public string? BtnText1 { get; set; }
  public string? BtnText2 { get; set; }
  public string? BtnLink1 { get; set; }
  public string? BtnLink2 { get; set; }
  public ImagePosition ImagePosition { get; set; }
  public int Order { get; set; }
}