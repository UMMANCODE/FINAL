namespace Final_Core.Entities;
public class Feature : AuditEntity {
  public string? IconLink { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public List<HouseFeature> Houses { get; set; } = [];
}
