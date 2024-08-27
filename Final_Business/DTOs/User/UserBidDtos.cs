namespace Final_Business.DTOs.User;

public record UserBidGetDto(
  int Id,
  string AppUserId,
  int HouseId,
  decimal Amount,
  DateTime CreatedAt
);

public record UserBidCreateDto(
  int HouseId,
  decimal Amount
);

