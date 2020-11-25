using System;
using AirQualityService.Model;

namespace AirQualityService.Data.Interface
{
    public interface IReportAirQualityInDayRepository
    {
        public void AddAQI(ReportAirQualityByDate report);
        public ReportAirQualityByDate GetAQI(Guid pointId, DateTime date);
    }
}
