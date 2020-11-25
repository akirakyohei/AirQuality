using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AirQualityService.Data;
using AirQualityService.Model;
using AirQualityService.Setting;
using MongoDB.Driver;

namespace AirQualityService.Helpers
{
    class Tinh
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string type { get; set; }

    }

    public interface IInitiallizeData
    {
        public Task InitialCollections();
    }
    public class InitiallizeData : IInitiallizeData
    {
        private readonly AirQualityContext _airQualityContext;


        public InitiallizeData(AirQualityContext airQualityContext)
        {
            _airQualityContext = airQualityContext;

        }

        [Obsolete]
        public async Task InitialCollections()
        {


            if (!_airQualityContext.CheckCollectionExist("City"))
            {
                _airQualityContext.CreateCollection("City");
                const string PATH_CITY = "./Resourse/Tinh.json";
                var tinhs = ReadFileJson(PATH_CITY);

                List<City> cities = new List<City>();
                foreach (var tinh in tinhs)
                {
                    var city = new City { NameCity = tinh.name, Type = tinh.type };
                    cities.Add(city);
                }
                await _airQualityContext.Cities.InsertManyAsync(cities);
            }

            if (!_airQualityContext.CheckCollectionExist("AirQuality"))
            {
                _airQualityContext.CreateCollection("AirQuality");

            }
            if (!_airQualityContext.CheckCollectionExist("Point"))
            {
                _airQualityContext.CreateCollection("Point");

            }
            if (!_airQualityContext.CheckCollectionExist("ReportAirQualityByDate"))
            {
                _airQualityContext.CreateCollection("ReportAirQualityByDate");
            }



        }

        private List<Tinh> ReadFileJson(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();

                    var result = System.Text.Json.JsonSerializer.Deserialize<List<Tinh>>(json);
                    return result;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
