using System;
using AirQualityService.Data.Interface;
using AirQualityService.Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AirQualityService.Data
{
    public class ReportAirQualityRepository : IReportAirQualityInDayRepository
    {
        private readonly IMongoCollection<ReportAirQualityByDate> reportCollection;

        public ReportAirQualityRepository(AirQualityContext context)
        {
            reportCollection = context.ReportAirQualityByDate;
        }

        public void AddAQI(ReportAirQualityByDate report)
        {
            reportCollection.InsertOne(report);
        }

        public ReportAirQualityByDate GetAQI(Guid pointId, DateTime date)
        {
            var result = (from a in reportCollection.AsQueryable()
                          where a.PointId.Equals(pointId) && a.DateTime == date
                          select a).FirstOrDefault();
            return result;
        }
    }
}
