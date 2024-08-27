namespace Final_Business.DTOs.General;

public record SliderGetOneDto(
    int Id,
    string Title,
    string SubTitle1,
    string SubTitle2,
    string Description,
    string ImageLink,
    string BtnText1,
    string BtnText2,
    string BtnLink1,
    string BtnLink2,
    ImagePosition ImagePosition,
    int Order
);

public record SliderGetAllDto(
     int Id,
     string Title,
     string ImageLink,
     int Order
);

public record SliderCreateDto(
  string Title,
  string SubTitle1,
  string SubTitle2,
  string Description,
  IFormFile Image,
  string? BtnText1,
  string? BtnText2,
  string? BtnLink1,
  string? BtnLink2,
  ImagePosition ImagePosition,
  int Order
);

public record SliderUpdateDto(
  string Title,
  string SubTitle1,
  string SubTitle2,
  string Description,
  IFormFile? Image,
  string? BtnText1,
  string? BtnText2,
  string? BtnLink1,
  string? BtnLink2,
  ImagePosition ImagePosition,
  int Order
);
