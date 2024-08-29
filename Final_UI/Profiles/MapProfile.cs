namespace Final_UI.Profiles;

public class MapProfile : Profile {

  public MapProfile() {
    CreateMap<FeatureUpdateRequest, FeatureResponse>().ReverseMap();
    CreateMap<FeatureCreateRequest, FeatureResponse>().ReverseMap();

    CreateMap<SliderUpdateRequest, SliderResponse>().ReverseMap();
    CreateMap<SliderCreateRequest, SliderResponse>().ReverseMap();

    CreateMap<HouseUpdateRequest, HouseResponse>()
      .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.SelectedFeatures.Select(id => new HouseFeatureResponse { Id = id }).ToList()))
      .ReverseMap()
      .ForMember(dest => dest.SelectedFeatures, opt => opt.MapFrom(src => src.Features.Select(f => f.Id).ToList()));

    CreateMap<HouseCreateRequest, HouseResponse>()
      .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.SelectedFeatures.Select(id => new HouseFeatureResponse { Id = id }).ToList()))
      .ReverseMap()
      .ForMember(dest => dest.SelectedFeatures, opt => opt.MapFrom(src => src.Features.Select(f => f.Id).ToList()));
  }
}