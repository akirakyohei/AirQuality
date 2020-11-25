using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AirQualityService.Model
{
    public class City
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid CityId { get; set; }
        public string NameCity { get; set; }
        public string Type { get; set; }

    }
}
