using AutoMapper;
using Final_UI.Models.Requests;
using Final_UI.Models.Responses;

namespace Final_UI.Profiles;

public class MapProfile : Profile {

  public MapProfile() {
    CreateMap<FeatureUpdateRequest, FeatureResponse>().ReverseMap();
    CreateMap<FeatureCreateRequest, FeatureResponse>().ReverseMap();

    CreateMap<SliderUpdateRequest, SliderResponse>().ReverseMap();
    CreateMap<SliderCreateRequest, SliderResponse>().ReverseMap();
  }
}