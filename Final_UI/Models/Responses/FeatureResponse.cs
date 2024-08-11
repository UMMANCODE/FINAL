using System.ComponentModel.DataAnnotations;

namespace Final_UI.Models.Responses;

public class FeatureResponse {
  public int Id { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public string? IconLink { get; set; }
}
