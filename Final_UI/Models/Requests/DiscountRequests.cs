using Final_UI.Helpers.Attributes;

namespace Final_UI.Models.Requests;

public class DiscountCreateRequest {
  [Required]
  public string Code { get; set; } = null!;
  [Required]
  public decimal Amount { get; set; }
  [Required]
  [FutureDate]
  public DateTime ExpiryDate { get; set; }
  [Required]
  public int HouseId { get; set; }
}
