using System;
namespace AirQualityService.Setting
{
    public interface IAirQualityDatabaseSettings
    {
        string AirQualityCollectionName { get; set; }
        string CityCollectionName { get; set; }
        string PointCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ReportAirQualityByDate { get; set; }

    }

    public class AirQualityDatabaseSettings : IAirQualityDatabaseSettings
    {
        public string AirQualityCollectionName { get; set; }
        public string CityCollectionName { get; set; }
        public string PointCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ReportAirQualityByDate { get; set; }
    }
}
