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
            CreateMap<CityVM, City>().ReverseMap();
            CreateMap<CityDetailVM, City>().ReverseMap();
            CreateMap<PointVM, Point>().ReverseMap();
            CreateMap<PointDetailVM, Point>().ReverseMap();
            CreateMap<AirQualityVM, AirQuality>().ReverseMap();
        }
    }
}
