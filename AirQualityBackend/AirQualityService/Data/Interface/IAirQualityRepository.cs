using System;
using System.Collections.Generic;
using AirQualityService.ViewModels;

namespace AirQualityService.Data.Interface
{
    public interface IAirQualityRepository
    {
        public List<AirQualityVM> GetAirQualitiesByPointId(int pointId, DateTime dateTimeFrom, int limit);
        public AirQualityVM GetAirQualityByPointId(int pointId, DateTime dateTimeFrom);
        public AirQualityVM GetAirQualityCurrentByPointId(int pointId);



    }
}
