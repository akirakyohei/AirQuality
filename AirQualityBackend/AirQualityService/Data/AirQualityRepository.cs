using System;
using System.Collections.Generic;
using AirQualityService.Data.Interface;
using AirQualityService.model;
using AirQualityService.Setting;
using AirQualityService.ViewModels;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;

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


        public List<AirQualityVM> GetAirQualitiesByPointId(int pointId, DateTime dateTimeFrom, int limit)
        {

            var result = _airQuality.Find(x => x.PointId == pointId && x.DateTime > dateTimeFrom)
                                   .SortBy(x => x.PointId)
                                   .Limit(limit).ToList();
            List<AirQualityVM> airQualityVMs = new List<AirQualityVM>();
            foreach (var airQ in result)
            {
                airQualityVMs.Add(_mapper.Map<AirQualityVM>(airQ));
            }
            return airQualityVMs;
        }

        public AirQualityVM GetAirQualityByPointId(int pointId, DateTime dateTimeFrom)
        {
            var result = _airQuality.Find(x => x.PointId == pointId && x.DateTime == dateTimeFrom).FirstOrDefault();
            if (result != null)
            {
                return _mapper.Map<AirQualityVM>(result);
            }

            return null;
        }

        public AirQualityVM GetAirQualityCurrentByPointId(int pointId)
        {
            var result = _airQuality.Find(x => x.PointId == pointId).SortBy(x => x.DateTime).FirstOrDefault();
            if (result != null)
            {
                return _mapper.Map<AirQualityVM>(result);
            }
            return null;
        }
    }
}
