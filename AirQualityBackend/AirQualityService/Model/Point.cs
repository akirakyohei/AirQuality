using System;
using System.Collections.Generic;
using AirQualityService.model;
using MongoDB.Bson.Serialization.Attributes;

namespace AirQualityService.Model
{
    public class Point
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        public int PointId { get; set; }
        public int CityId { get; set; }
        public string NameLocation { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }

    }
}
