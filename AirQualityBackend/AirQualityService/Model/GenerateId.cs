using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Operations;

namespace AirQualityService.Model
{
    public class GenerateId : IIdGenerator
    {
        private static Int64 count = 0;
        public bool IsEmpty(object id)
        {
            return id == null | (Int64)id == 0;
        }

        object IIdGenerator.GenerateId(object container, object document)
        {
            count++;
            return count;
        }
    }
}
