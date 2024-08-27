namespace Final_Business.Profiles;

public class MapProfile : Profile {
  public MapProfile(IHttpContextAccessor accessor) {
    var context = accessor.HttpContext;

    var uriBuilder = new UriBuilder(context!.Request.Scheme, context.Request.Host.Host, context.Request.Host.Port ?? -1);
    if (uriBuilder.Uri.IsDefaultPort) uriBuilder.Port = -1;
    var baseUrl = uriBuilder.Uri.AbsoluteUri;

    CreateMap<House, AdminHouseUpdateDto>().ReverseMap()
      .ForMember(dest => dest.HouseImages, opt => opt.Ignore());

    CreateMap<House, AdminHouseCreateDto>().ReverseMap()
      .ForMember(dest => dest.HouseImages, opt => opt.Ignore());

    CreateMap<House, AdminHouseGetAllDto>().ReverseMap();
    CreateMap<House, AdminHouseGetOneDto>().ReverseMap();

    CreateMap<House, UserHouseUpdateDto>().ReverseMap()
      .ForMember(dest => dest.HouseImages, opt => opt.Ignore());

    CreateMap<House, UserHouseCreateDto>().ReverseMap()
      .ForMember(dest => dest.HouseImages, opt => opt.Ignore());

    CreateMap<House, UserHouseGetAllDto>().ReverseMap();
    CreateMap<House, UserHouseGetOneDto>().ReverseMap();

    CreateMap<Slider, SliderCreateDto>().ReverseMap();
    CreateMap<Slider, SliderUpdateDto>().ReverseMap();

    CreateMap<Feature, FeatureCreateDto>().ReverseMap();
    CreateMap<Feature, FeatureUpdateDto>().ReverseMap();

    CreateMap<Comment, CommentGetDto>()
      .ForMember(dest => dest.AppUserAvatarLink, opt => opt.MapFrom(src => src.AppUser.AvatarLink))
      .ForMember(dest => dest.AppUserUserName, opt => opt.MapFrom(src => src.AppUser.UserName))
      .ReverseMap();
    CreateMap<Comment, CommentCreateDto>().ReverseMap();

    CreateMap<Order, OrderGetDto>()
      .ForMember(dest => dest.AppUserAvatarLink, opt => opt.MapFrom(src => src.AppUser.AvatarLink))
      .ForMember(dest => dest.AppUserUserName, opt => opt.MapFrom(src => src.AppUser.UserName))
      .ReverseMap();
    CreateMap<Order, OrderCreateDto>().ReverseMap();

    CreateMap<Bid, UserBidGetDto>().ReverseMap();
    CreateMap<Bid, UserBidCreateDto>().ReverseMap();

    CreateMap<Discount, DiscountGetDto>().ReverseMap();
    CreateMap<Discount, DiscountCreateDto>().ReverseMap();

    CreateMap<AppUser, UserRegisterDto>().ReverseMap();
    CreateMap<AppUser, UserCreateAdminDto>().ReverseMap();

    CreateMap<HouseImage, HouseImageGetDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/houses/" + src.ImageLink))
      .ReverseMap();

    CreateMap<Slider, SliderGetOneDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/sliders/" + src.ImageLink))
      .ReverseMap();

    CreateMap<Slider, SliderGetAllDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/sliders/" + src.ImageLink))
      .ReverseMap();

    CreateMap<Feature, FeatureGetAllDto>()
      .ForCtorParam("IconLink", opt => opt.MapFrom(src => baseUrl + "images/features/" + src.IconLink))
      .ReverseMap();

    CreateMap<Feature, FeatureGetOneDto>()
      .ForCtorParam("IconLink", opt => opt.MapFrom(src => baseUrl + "images/features/" + src.IconLink))
      .ReverseMap();
  }
}