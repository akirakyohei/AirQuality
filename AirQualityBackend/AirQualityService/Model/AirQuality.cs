using System;
using AirQualityService.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AirQualityService.model
{
    public class AirQuality
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid AirQualityId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid PointId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local, Representation = MongoDB.Bson.BsonType.DateTime)]
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
