namespace Final_Business.DTOs.General;

public record DiscountGetDto(
  int Id,
  string Code,
  decimal Amount,
  DateTime ExpiryDate,
  int HouseId
);

public record DiscountCreateDto(
  string Code,
  decimal Amount,
  DateTime ExpiryDate,
  int HouseId
);
