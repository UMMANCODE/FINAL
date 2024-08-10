using Final_Business.DTOs.General;
using Final_Business.DTOs.User;
using Final_Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Final_Business.DTOs.Admin;

public record AdminHouseGetOneDto(
  int Id,
  string OwnerId,
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
  List<HouseImageGetDto> Images,
  List<CommentGetDto> Comments,
  List<UserBidGetDto> Bids,
  List<DiscountGetDto> Discounts,
  PropertyStatus Status,
  PropertyType Type,
  PropertyState State
);

public record AdminHouseGetAllDto(
  int Id,
  string OwnerId,
  string Name,
  decimal Price
);

public record AdminHouseCreateDto(
  string Name,
  string OwnerId,
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
  bool IsAdmin = true
);

public record AdminHouseUpdateDto(
  string Name,
  string OwnerId,
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
  List<int> IdsToDelete,
  bool IsAdmin = true
);
