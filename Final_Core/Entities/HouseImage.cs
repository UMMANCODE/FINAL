namespace Final_Core.Entities;
public class HouseImage : BaseEntity {
  public string? ImageLink { get; set; }
  public int HouseId { get; set; }
  public House? House { get; set; }
  public bool IsMain { get; set; }
}
