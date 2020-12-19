using System;
namespace AirQualityService.ViewModels
{
    public class AirQualityVM
    {
        public string PointId { get; set; }

        public DateTime DateTime { get; set; }

        public float Temperature { get; set; }

        public float Humidity { get; set; }

        public float O3 { get; set; }

        public float CO { get; set; }

        public float NO2 { get; set; }

        public float SO2 { get; set; }

        public float PM2_5 { get; set; }

        public float PM10_0 { get; set; }

        public int AQIInHour { get; set; }
    }
}
