namespace Final_Core.Entities;
public class Discount {
  public int Id { get; set; }
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpiryDate { get; set; }
  public int HouseId { get; set; }
  public House House { get; set; }
}
