using Final_Business.DTOs.General;
using Final_Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Final_Business.DTOs.User;

public record UserHouseGetOneDto(
  int Id,
  bool IsFeatured,
  string Name,
  string Description,
  string Location,
  decimal Price,
  string PropertyId,
  float HomeArea,
  byte Rooms,
  byte Bedrooms,
  byte Bathrooms,
  int BuiltYear,
  List<HouseImageGetDto> HouseImages,
  List<CommentGetDto> Comments,
  List<UserBidGetDto> Bids,
  List<DiscountGetDto> Discounts,
  PropertyStatus Status,
  PropertyType Type,
  PropertyState State
);

public record UserHouseGetAllDto(
  int Id,
  string Name,
  string Location,
  float HomeArea,
  byte Bedrooms,
  byte Bathrooms,
  decimal Price,
  List<HouseImageGetDto> HouseImages
);

public record UserHouseCreateDto(
  string Name,
  string Description,
  string Location,
  decimal Price,
  float HomeArea,
  byte Rooms,
  byte Bedrooms,
  byte Bathrooms,
  int BuiltYear,
  bool IsFeatured,
  PropertyStatus Status,
  PropertyType Type,
  PropertyState State,
  List<IFormFile> Images,
  bool IsAdmin = false
);

public record UserHouseUpdateDto(
  string Name,
  string Description,
  string Location,
  decimal Price,
  float HomeArea,
  byte Rooms,
  byte Bedrooms,
  byte Bathrooms,
  int BuiltYear,
  bool IsFeatured,
  PropertyStatus Status,
  PropertyType Type,
  PropertyState State,
  List<IFormFile>? Images,
  List<int>? IdsToDelete,
  bool IsAdmin = false
);
