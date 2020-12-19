using System;
using System.Collections.Generic;
using AirQualityService.Data.Interface;
using AirQualityService.Model;
using AirQualityService.Setting;
using AirQualityService.ViewModels;
using MongoDB.Driver;
using System.Linq;
using AutoMapper;

namespace AirQualityService.Data
{
    public class CityRepository : ICityReporitory
    {

        private readonly IMongoCollection<City> _cities;
        private readonly IMapper _mapper;

        public CityRepository(AirQualityContext context, IMapper mapper)
        {
            _cities = context.Cities;
            _mapper = mapper;
        }



        public List<CityDetailVM> GetCities()
        {
            var cities = _cities.Find(city => true).ToList();
            List<CityDetailVM> cityDetailVMs = new List<CityDetailVM>();
            foreach (var city in cities)
            {
                cityDetailVMs.Add(_mapper.Map<CityDetailVM>(city));
            }
            return cityDetailVMs;
        }


        public CityDetailVM GetCityDetailbyName(string nameCity)
        {
            var city = _cities.Find(city => city.NameCity == nameCity).FirstOrDefault();

            return _mapper.Map<CityDetailVM>(city);

        }

        public List<CityVM> GetNameCities()
        {
            var cities = _cities.Find(city => true).ToList();
            List<CityVM> cityDetailVMs = new List<CityVM>();
            foreach (var city in cities)
            {
                cityDetailVMs.Add(_mapper.Map<CityVM>(city));
            }
            return cityDetailVMs;
        }

        public string GetNameCityById(Guid id)
        {
            var city = _cities.Find(city => city.CityId.Equals(id)).FirstOrDefault();
            if (city != null)
            {
                return city.NameCity;
            }
            return null;
        }
    }
}
