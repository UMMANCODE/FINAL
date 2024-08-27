namespace Final_UI.Models.Requests;

public class DiscountCreateRequest {
  [Required]
  public string Code { get; set; } = null!;
  [Required]
  public decimal Amount { get; set; }
  [Required]
  public DateTime ExpiryDate { get; set; }
  [Required]
  public int HouseId { get; set; }
}
