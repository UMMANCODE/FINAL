namespace Final_Business.DTOs.General;

public record OrderGetDto(
  int Id,
  decimal Price,
  DateTime CreatedAt,
  string Address,
  OrderStatus Status,
  int HouseId,
  string AppUserId,
  string AppUserAvatarLink,
  string AppUserUserName
);

public record OrderCreateDto(
  string Address,
  int HouseId
);
