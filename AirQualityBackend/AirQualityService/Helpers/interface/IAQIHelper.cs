using System;
using System.Collections.Generic;
using AirQualityService.model;

namespace AirQualityService.Helpers.@interface
{
    public enum AirType
    {
        O3, CO2, NO3, PM2_5, PM10_0
    }
    public interface IAQIHelper
    {

        public int GetAQIInHour(AirQuality air);

        public int GetAQIInDay(Guid idPoint);

    }
}
