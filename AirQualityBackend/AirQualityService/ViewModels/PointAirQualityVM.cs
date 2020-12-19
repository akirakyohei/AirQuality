using System;
namespace AirQualityService.ViewModels
{
    public class PointAirQualityVM
    {
        public Guid pointId { get; set; }
        public string pointName { get; set; }
        public string pointAddress { get; set; }
        public int aqi { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

    }
}
