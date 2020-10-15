using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirQualityService.Model
{
    public class City
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public int CityId { get; set; }
        public string NameCity { get; set; }
        public string Type { get; set; }

    }
}
