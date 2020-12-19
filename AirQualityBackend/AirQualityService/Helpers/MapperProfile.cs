using System;
using AirQualityService.model;
using AirQualityService.Model;
using AirQualityService.ViewModels;
using AutoMapper;

namespace AirQualityService.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CityVM, City>()
                .ReverseMap();
            CreateMap<CityDetailVM, City>()
                .ForMember(des => des.CityId, opt => opt.MapFrom(src => String.IsNullOrWhiteSpace(src.CityId) ? (Guid?)null : Guid.Parse(src.CityId)));
            CreateMap<City, CityDetailVM>()
                .ForMember(des => des.CityId, opt => opt.MapFrom(src => src.CityId.ToString()));


            CreateMap<PointVM, Point>()
                 .ForMember(des => des.CityId, opt => opt.MapFrom(src => String.IsNullOrWhiteSpace(src.CityId) ? (Guid?)null : Guid.Parse(src.CityId)))
                 .ForMember(des => des.PointId, opt => opt.Ignore());

            CreateMap<Point, PointVM>()
                 .ForMember(des => des.CityId, opt => opt.MapFrom(src => src.PointId.ToString()));




            CreateMap<PointDetailVM, Point>()
                 .ForMember(des => des.PointId, opt => opt.MapFrom(src => String.IsNullOrWhiteSpace(src.PointId) ? (Guid?)null : Guid.Parse(src.PointId)));
            CreateMap<Point, PointDetailVM>()
              .ForMember(des => des.PointId, opt => opt.MapFrom(src => src.PointId.ToString()));


            CreateMap<AirQualityVM, AirQuality>()
                .ReverseMap();

        }
    }
}
