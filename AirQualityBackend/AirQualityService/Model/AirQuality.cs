using System;
using AirQualityService.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace AirQualityService.model
{
    public class AirQuality
    {
        [BsonId(IdGenerator = typeof(GenerateId))]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]

        public Int64 AirQualityId { get; set; }


        public int PointId { get; set; }


        public DateTime DateTime { get; set; }

        public float Temperature { get; set; }

        public float Humidity { get; set; }

        public float PPM { get; set; }

        public int PM1_0 { get; set; }

        public int PM2_5 { get; set; }

        public int PM10_0 { get; set; }


    }
}
