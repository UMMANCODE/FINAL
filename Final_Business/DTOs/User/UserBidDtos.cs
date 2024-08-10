namespace Final_Business.DTOs.User;

public record UserBidGetDto(
  int Id,
  string UserId,
  int HouseId,
  decimal Amount,
  DateTime CreatedAt
);

public record UserBidCreateDto(
  string UserId,
  int HouseId,
  decimal Amount
);

