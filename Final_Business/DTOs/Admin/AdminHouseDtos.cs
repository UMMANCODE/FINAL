namespace Final_Business.DTOs.Admin;

public record AdminHouseGetOneDto(
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
  List<HouseFeatureGetDto> Features,
  List<OrderGetDto> Orders,
  PropertyStatus Status,
  PropertyType Type,
  PropertyState State
);

public record AdminHouseGetAllDto(
  int Id,
  string Location,
  string Name,
  decimal Price,
  PropertyStatus Status,
  List<DiscountGetDto> Discounts,
  List<HouseImageGetDto> HouseImages
);

public record AdminHouseCreateDto(
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
  List<int> SelectedFeatures,
  bool IsAdmin = true
);

public record AdminHouseUpdateDto(
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
  List<int> SelectedFeatures,
  List<int>? IdsToDelete,
  bool IsAdmin = true
);
