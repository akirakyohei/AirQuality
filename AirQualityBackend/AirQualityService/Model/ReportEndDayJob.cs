using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.Helpers.@interface;
using Quartz;

namespace AirQualityService.Model
{
    public class ReportEndDayJob : IJob
    {
        private IReportAirQualityInDayRepository reportAirQuality;
        private IAQIHelper aQIHelper;
        private IPointRepository pointRepository;

        public ReportEndDayJob(IReportAirQualityInDayRepository reportAirQuality, IAQIHelper aQIHelper, IPointRepository pointRepository)
        {
            this.reportAirQuality = reportAirQuality;
            this.aQIHelper = aQIHelper;
            this.pointRepository = pointRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("job 2");

            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            List<Guid> points = pointRepository.GetListPointIds();

            foreach (var idPoint in points)
            {
                var aqi = aQIHelper.GetAQIInDay(idPoint);
                Console.WriteLine("aqi " + aqi);
                ReportAirQualityByDate report = new ReportAirQualityByDate
                {
                    AQI = aqi,
                    DateTime = date,
                    PointId = idPoint
                };
                reportAirQuality.AddAQI(report);
            }
            return Task.CompletedTask;
        }
    }
}
