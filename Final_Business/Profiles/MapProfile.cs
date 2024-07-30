﻿using AutoMapper;
using Final_Business.DTOs.Admin;
using Final_Business.DTOs.General;
using Final_Business.DTOs.User;
using Final_Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Final_Business.Profiles;

public class MapProfile : Profile {
  public MapProfile(IHttpContextAccessor accessor) {
    var context = accessor.HttpContext;

    var uriBuilder = new UriBuilder(context!.Request.Scheme, context.Request.Host.Host, context.Request.Host.Port ?? -1);
    if (uriBuilder.Uri.IsDefaultPort) uriBuilder.Port = -1;
    var baseUrl = uriBuilder.Uri.AbsoluteUri;

    CreateMap<House, AdminHouseUpdateDto>().ReverseMap()
      .ForMember(dest => dest.Images, opt => opt.Ignore());

    CreateMap<House, AdminHouseCreateDto>().ReverseMap()
      .ForMember(dest => dest.Images, opt => opt.Ignore());

    CreateMap<House, AdminHouseGetAllDto>().ReverseMap();
    CreateMap<House, AdminHouseGetOneDto>().ReverseMap();

    CreateMap<House, UserHouseUpdateDto>().ReverseMap()
      .ForMember(dest => dest.Images, opt => opt.Ignore());

    CreateMap<House, UserHouseCreateDto>().ReverseMap()
      .ForMember(dest => dest.Images, opt => opt.Ignore());

    CreateMap<House, UserHouseGetAllDto>().ReverseMap();
    CreateMap<House, UserHouseGetOneDto>().ReverseMap();

    CreateMap<Slider, SliderCreateDto>().ReverseMap();
    CreateMap<Slider, SliderUpdateDto>().ReverseMap();
    CreateMap<Slider, SliderGetAllDto>().ReverseMap();
    CreateMap<Slider, SliderGetOneDto>().ReverseMap();

    CreateMap<Feature, FeatureCreateDto>().ReverseMap();
    CreateMap<Feature, FeatureUpdateDto>().ReverseMap();
    CreateMap<Feature, FeatureGetAllDto>().ReverseMap();
    CreateMap<Feature, FeatureGetOneDto>().ReverseMap();

    CreateMap<Comment, CommentGetDto>().ReverseMap();
    CreateMap<Comment, CommentCreateDto>().ReverseMap();

    CreateMap<HouseImage, HouseImageGetDto>().ReverseMap();

    CreateMap<HouseImage, HouseImageGetDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/houses/" + src.ImageLink));

    CreateMap<Slider, SliderGetOneDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/sliders/" + src.ImageLink));

    CreateMap<Slider, SliderGetAllDto>()
      .ForCtorParam("ImageLink", opt => opt.MapFrom(src => baseUrl + "images/sliders/" + src.ImageLink));

    CreateMap<Feature, FeatureGetAllDto>()
      .ForCtorParam("IconLink", opt => opt.MapFrom(src => baseUrl + "images/features/" + src.IconLink));

    CreateMap<Feature, FeatureGetOneDto>()
      .ForCtorParam("IconLink", opt => opt.MapFrom(src => baseUrl + "images/features/" + src.IconLink));
  }
}