using System;
using System.Collections.Generic;
using System.Linq;
using AirQualityService.Data.Interface;
using AirQualityService.model;
using AirQualityService.Setting;
using AirQualityService.ViewModels;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AirQualityService.Data
{
    public class AirQualityRepository : IAirQualityRepository
    {
        private readonly IMongoCollection<AirQuality> _airQuality;
        private readonly IMapper _mapper;

        public AirQualityRepository(AirQualityContext context, IMapper mapper)
        {
            _airQuality = context.AirQualities;
            _mapper = mapper;
        }


        public List<AirQualityVM> GetAirQualitiesByPointId(Guid pointId, DateTime dateTimeFrom, int limit)
        {

            var result = _airQuality.Find(x => x.PointId.Equals(pointId) && x.DateTime >= dateTimeFrom)
                                   .SortBy(x => x.DateTime)
                                   .Limit(limit).ToList();
            List<AirQualityVM> airQualityVMs = new List<AirQualityVM>();
            foreach (var airQ in result)
            {
                airQualityVMs.Add(_mapper.Map<AirQualityVM>(airQ));
            }
            return airQualityVMs;
        }

        public List<AirQualityVM> GetAirQualitiesByPointId(Guid pointId)
        {
            var result = _airQuality.Find(x => x.PointId.Equals(pointId))
                                  .SortBy(x => x.DateTime)
                                 .ToList();
            List<AirQualityVM> airQualityVMs = new List<AirQualityVM>();
            foreach (var airQ in result)
            {
                airQualityVMs.Add(_mapper.Map<AirQualityVM>(airQ));
            }
            return airQualityVMs;
        }

        public List<AirQuality> GetAirQualitiesInDayByPointId(DateTime date, Guid pointId)
        {
            var result = (from a in _airQuality.AsQueryable()
                          where a.PointId.Equals(pointId) && a.DateTime >= date && a.DateTime <= date.AddDays(1)
                          orderby a.DateTime
                          select a).ToList();
            return result;
        }

        public AirQualityVM GetAirQualityByPointId(Guid pointId, DateTime dateTime)
        {
            Console.WriteLine(dateTime.ToString());

            var result = (from a in _airQuality.AsQueryable()
                          where a.PointId.Equals(pointId) && a.DateTime == dateTime
                          select a).FirstOrDefault();
            if (result != null)
            {
                return _mapper.Map<AirQualityVM>(result);
            }

            return null;
        }

        public AirQualityVM GetAirQualityCurrentByPointId(Guid pointId)
        {
            try
            {
                var result = _airQuality.Find(x => x.PointId.Equals(pointId)).SortByDescending(x => x.DateTime).FirstOrDefault();
                Console.WriteLine("AirQualityCurrentByPointId" + result);
                if (result != null)
                {
                    return _mapper.Map<AirQualityVM>(result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public List<AirQuality> GetAirQualityNowLimit(Guid pointId, int limit = 1)
        {
            var airQualities = (from a in _airQuality.AsQueryable()
                                where a.PointId.Equals(pointId)
                                orderby a.DateTime
                                select a
                              );

            if (airQualities.Count() > limit)
            {
                airQualities = (IOrderedMongoQueryable<AirQuality>)airQualities.Skip(airQualities.Count() - limit);


            }
            return airQualities.ToList();
        }

        public List<float> GetPM10_0ByDate(DateTime date, Guid pointId)
        {

            var airQualities = (from a in _airQuality.AsQueryable()
                                where (a.DateTime >= date && a.PointId.Equals(pointId))
                                select a).ToList();
            List<float> result = new List<float>();

            foreach (var item in airQualities)
            {
                result.Add(item.PM10_0);
            }

            return result;
        }



        public List<float> GetPM2_5ByDate(DateTime date, Guid pointId)
        {
            var airQualities = (from a in _airQuality.AsQueryable()
                                where a.DateTime >= date
                                select a).ToList();
            List<float> result = new List<float>();

            foreach (var item in airQualities)
            {
                result.Add(item.PM2_5);
            }

            return result;
        }


    }
}
