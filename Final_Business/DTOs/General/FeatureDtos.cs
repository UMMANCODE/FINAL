namespace Final_Business.DTOs.General;

public record FeatureGetOneDto(
  int Id,
  string Name,
  string Description,
  string IconLink
);

public record FeatureGetAllDto(
  int Id,
  string Name,
  string Description,
  string IconLink
);

public record FeatureCreateDto(
  string Name,
  string Description,
  IFormFile Icon
);

public record FeatureUpdateDto(
  string Name,
  string Description,
  IFormFile? Icon
);