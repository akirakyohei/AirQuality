using System;
using System.Collections.Generic;
using AirQualityService.model;
using AirQualityService.ViewModels;

namespace AirQualityService.Data.Interface
{
    public interface IAirQualityRepository
    {
        public List<AirQualityVM> GetAirQualitiesByPointId(Guid pointId, DateTime dateTimeFrom, int limit);
        public List<AirQualityVM> GetAirQualitiesByPointId(Guid pointId);
        public AirQualityVM GetAirQualityByPointId(Guid pointId, DateTime dateTimeFrom);
        public AirQualityVM GetAirQualityCurrentByPointId(Guid pointId);
        public List<AirQuality> GetAirQualitiesInDayByPointId(DateTime date, Guid pointId);
        public List<float> GetPM2_5ByDate(DateTime date, Guid pointId);
        public List<float> GetPM10_0ByDate(DateTime date, Guid pointId);
        public List<AirQuality> GetAirQualityNowLimit(Guid pointId, int limit = 1);


    }
}
