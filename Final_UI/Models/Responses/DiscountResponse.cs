namespace Final_UI.Models.Responses;

public class DiscountResponse {
  public int Id { get; set; }
  public string Code { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public DateTime ExpiryDate { get; set; }
  public int HouseId { get; set; }
}
