using System;
using System.Collections.Generic;
using System.Linq;
using AirQualityService.Data.Interface;
using AirQualityService.model;
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

        public List<ReportAirQualityByDate> GetAirQualityNowLimit(Guid pointId, int limit = 1)
        {
            var airQualities = (from a in reportCollection.AsQueryable()
                                where a.PointId.Equals(pointId)
                                orderby a.DateTime
                                select a);

            if (limit < 1)
            {
                return airQualities.ToList();
            }

            if (airQualities.Count() > limit)
            {
                airQualities = (IOrderedMongoQueryable<ReportAirQualityByDate>)airQualities.Skip(airQualities.Count() - limit);


            }
            return airQualities.ToList();
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
