using System;
using System.Collections.Generic;
using AirQualityService.ViewModels;

namespace AirQualityService.Data.Interface
{
    public interface ICityReporitory
    {
        public List<CityVM> GetNameCities();
        public List<CityDetailVM> GetCities();
        public CityDetailVM GetCityDetailbyName(string nameCity);
        public string GetNameCityById(int id);
    }
}
