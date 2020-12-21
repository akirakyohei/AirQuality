using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AirQualityService.Model
{
    public class Account
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime Modified { get; set; }

    }
}
