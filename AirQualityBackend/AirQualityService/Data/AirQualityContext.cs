using System;
using System.Collections.Generic;
using AirQualityService.model;
using AirQualityService.Model;
using AirQualityService.Setting;
using Microsoft.EntityFrameworkCore.Internal;
using MongoDB.Driver;

namespace AirQualityService.Data
{
    public class AirQualityContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly IAirQualityDatabaseSettings _setting;

        public AirQualityContext(IAirQualityDatabaseSettings setting)
        {

            _setting = setting;

            var client = new MongoClient(setting.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(setting.DatabaseName);
            }

        }

        public IMongoCollection<AirQuality> AirQualities
        {
            get
            {
                return _database.GetCollection<AirQuality>(_setting.AirQualityCollectionName);
            }
        }

        public IMongoCollection<City> Cities
        {
            get
            {
                return _database.GetCollection<City>(_setting.CityCollectionName);
            }
        }
        public IMongoCollection<Point> Points
        {
            get
            {
                return _database.GetCollection<Point>(_setting.PointCollectionName);
            }
        }

        public Boolean CheckCollectionExist(string name)
        {

            var listName = _database.ListCollectionNames().ToList();

            foreach (var colName in listName)
            {
                if (colName == name) return true;
            }

            return false;
        }
        public void CreateCollection(string name)
        {

            _database.CreateCollection(name);


        }
    }
}
