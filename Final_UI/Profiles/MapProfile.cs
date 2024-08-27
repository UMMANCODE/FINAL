namespace Final_UI.Profiles;

public class MapProfile : Profile {

  public MapProfile() {
    CreateMap<FeatureUpdateRequest, FeatureResponse>().ReverseMap();
    CreateMap<FeatureCreateRequest, FeatureResponse>().ReverseMap();

    CreateMap<SliderUpdateRequest, SliderResponse>().ReverseMap();
    CreateMap<SliderCreateRequest, SliderResponse>().ReverseMap();

    CreateMap<HouseUpdateRequest, HouseResponse>().ReverseMap();
    CreateMap<HouseCreateRequest, HouseResponse>().ReverseMap();
  }
}