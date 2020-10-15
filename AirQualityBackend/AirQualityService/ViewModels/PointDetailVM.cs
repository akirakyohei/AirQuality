using System;
namespace AirQualityService.ViewModels
{
    public class PointDetailVM
    {
        public int PointId { get; set; }
        public string CityName { get; set; }
        public string NameLocation { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
