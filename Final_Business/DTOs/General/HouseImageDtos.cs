namespace Final_Business.DTOs.General;

public record HouseImageGetDto(
  int Id,
  string ImageLink,
  int HouseId,
  bool IsMain
);
