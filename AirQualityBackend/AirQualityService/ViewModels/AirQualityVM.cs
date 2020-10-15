﻿using System;
namespace AirQualityService.ViewModels
{
    public class AirQualityVM
    {
        public int AirQualityId { get; set; }

        public DateTime DateTime { get; set; }

        public float Temperature { get; set; }

        public float Humidity { get; set; }

        public float PPM { get; set; }

        public int PM1_0 { get; set; }

        public int PM2_5 { get; set; }

        public int PM10_0 { get; set; }
    }
}
