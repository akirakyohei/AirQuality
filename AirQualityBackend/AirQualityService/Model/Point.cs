using System;
using System.Collections.Generic;
using AirQualityService.model;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace AirQualityService.Model
{
    public class Point
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid PointId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid CityId { get; set; }
        public string NameLocation { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }

    }
}
