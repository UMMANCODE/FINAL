namespace Final_UI.Models.Responses;

public class HouseImageResponse {
  public int Id { get; set; }
  public string? ImageLink { get; set; }
  public int HouseId { get; set; }
  public bool IsMain { get; set; }
}
