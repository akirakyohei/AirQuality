using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AirQualityService.Model
{
    public class ReportAirQualityByDate
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]

        [BsonRepresentation(BsonType.String)]
        public Guid ReportAirQualityId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid PointId { get; set; }


        public DateTime DateTime { get; set; }

        public int AQI { get; set; }

    }
}
