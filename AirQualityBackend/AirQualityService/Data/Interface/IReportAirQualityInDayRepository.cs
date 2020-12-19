using System;
using System.Collections.Generic;
using AirQualityService.Model;

namespace AirQualityService.Data.Interface
{
    public interface IReportAirQualityInDayRepository
    {
        public void AddAQI(ReportAirQualityByDate report);
        public ReportAirQualityByDate GetAQI(Guid pointId, DateTime date);
        public List<ReportAirQualityByDate> GetAirQualityNowLimit(Guid pointId, int limit = 1);
    }
}
