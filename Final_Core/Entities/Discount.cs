namespace Final_Core.Entities;
public class Discount : BaseEntity {
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpiryDate { get; set; }
  public int HouseId { get; set; }
  public House House { get; set; }
}
