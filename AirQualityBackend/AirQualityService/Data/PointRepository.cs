using System;
using System.Collections.Generic;
using AirQualityService.Data.Interface;
using AirQualityService.Model;
using AirQualityService.Setting;
using AirQualityService.ViewModels;
using AutoMapper;
using MongoDB.Driver;

namespace AirQualityService.Data
{
    public class PointRepository : IPointRepository
    {
        private readonly IMongoCollection<Point> _points;
        private readonly ICityReporitory _cityReporitory;
        private readonly IMapper _mapper;

        public PointRepository(AirQualityContext context, IMapper mapper, ICityReporitory cityReporitory)
        {
            _points = context.Points;
            _mapper = mapper;
            _cityReporitory = cityReporitory;
        }

        public Point AddPoint(PointDetailVM pointVM)
        {
            var point = _mapper.Map<Point>(pointVM);
            var city = _cityReporitory.GetCityDetailbyName(pointVM.CityName);
            if (city != null)
            {
                point.CityId = city.CityId;
                _points.InsertOne(point);
                return point;
            }
            return null;
        }

        public void DeletePointById(int id) => _points.DeleteOne(point => point.CityId == id);


        public PointDetailVM GetDetailPointById(int id)
        {
            Point point = _points.Find(p => p.PointId == id).FirstOrDefault();

            if (point == null)
                return null;

            var pointDetailVM = _mapper.Map<PointDetailVM>(point);
            var cityName = _cityReporitory.GetNameCityById(point.CityId);
            pointDetailVM.CityName = cityName;
            return pointDetailVM;
        }

        public List<PointVM> GetNamePoints()
        {
            List<Point> points = _points.Find(p => true).ToList();
            List<PointVM> pointVMs = new List<PointVM>();
            foreach (var p in points)
            {
                pointVMs.Add(_mapper.Map<PointVM>(p));
            }
            return pointVMs;
        }

        public void UpdatePoint(PointDetailVM pointVM, int id)
        {
            var point = _mapper.Map<Point>(pointVM);
            var city = _cityReporitory.GetCityDetailbyName(pointVM.CityName);
            if (city != null)
            {
                point.CityId = city.CityId;
                _points.ReplaceOne(p => p.CityId == id, point);
            }

        }
    }
}
