namespace Final_Core.Entities;
public class HouseFeature : BaseEntity {
  public int HouseId { get; set; }
  public House? House { get; set; }
  public int FeatureId { get; set; }
  public Feature? Feature { get; set; }
}
