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
        private readonly IAirQualityRepository _airQualityRepository;
        private readonly IMapper _mapper;


        public PointRepository(AirQualityContext context, ICityReporitory cityReporitory, IAirQualityRepository airQualityRepository, IMapper mapper)
        {
            _points = context.Points;
            _cityReporitory = cityReporitory;
            _airQualityRepository = airQualityRepository;
            _mapper = mapper;
        }


        public Point AddPoint(PointDetailVM pointVM)
        {
            var point = _mapper.Map<Point>(pointVM);
            var city = _cityReporitory.GetCityDetailbyName(pointVM.CityName);
            if (city != null)
            {
                point.CityId = Guid.Parse(city.CityId);
                _points.InsertOne(point);
                return point;
            }
            return null;
        }

        public void DeletePointById(Guid id) => _points.DeleteOne(point => point.PointId.Equals(id));

        public Guid GetCityIdComposePoint(Guid pointId)
        {
            var result = _points.Find(x => x.PointId == pointId).FirstOrDefault();
            if (result != null)
            {
                return result.CityId;
            }
            return Guid.Empty;
        }

        public PointDetailVM GetDetailPointById(Guid id)
        {
            Point point = _points.Find(p => p.PointId.Equals(id)).FirstOrDefault();

            if (point == null)
                return null;

            var pointDetailVM = _mapper.Map<PointDetailVM>(point);
            var cityName = _cityReporitory.GetNameCityById(point.CityId);
            pointDetailVM.CityName = cityName;
            return pointDetailVM;
        }

        public List<Guid> GetListPointIds()
        {
            var ar = _points.Find(p => true);
            var arr = ar.ToList();

            List<Guid> result = new List<Guid>();
            foreach (var item in arr)
            {
                result.Add(item.PointId);

            }
            return result;
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

        public List<PointAirQualityVM> GetPointAirQualities(Guid cityId)
        {
            var listPoint = _points.Find(x => x.CityId.Equals(cityId)).ToList();
            var result = new List<PointAirQualityVM>();
            if (listPoint != null)
            {
                foreach (var item in listPoint)
                {
                    var temp = new PointAirQualityVM()
                    {
                        pointId = item.PointId,
                        pointName = item.NameLocation,
                        pointAddress = item.Address,
                        lat = item.Lat,
                        lng = item.Lng
                    };
                    var airCurrent = _airQualityRepository.GetAirQualityCurrentByPointId(item.PointId);
                    if (airCurrent == null)
                    {
                        temp.aqi = 0;
                    }
                    else
                    {
                        temp.aqi = airCurrent.AQIInHour;
                    }

                    result.Add(temp);
                }
                return result;
            }
            return null;

        }

        public List<PointAirQualityVM> GetPointAirQualities()
        {
            Console.WriteLine("jhh");
            var points = _points.Find(x => true).ToList();
            Console.WriteLine("jhh");
            if (points != null)
            {
                List<PointAirQualityVM> result = new List<PointAirQualityVM>();

                foreach (var item in points)
                {
                    var temp = new PointAirQualityVM()
                    {
                        pointId = item.PointId,
                        pointName = item.NameLocation,
                        pointAddress = item.Address,
                        lat = item.Lat,
                        lng = item.Lng
                    };
                    var aqi = _airQualityRepository.GetAirQualityCurrentByPointId(item.PointId);
                    if (aqi != null)
                    {
                        temp.aqi = aqi.AQIInHour;
                    }
                    else
                    {
                        temp.aqi = 0;
                    }
                    result.Add(temp);
                }
                return result;

            }
            return null;
        }

        public List<PointDetailVM> GetPoints()
        {
            Console.WriteLine("jhh");
            var points = _points.Find(x => true).ToList();
            Console.WriteLine("jhh");
            if (points != null)
            {
                List<PointDetailVM> result = new List<PointDetailVM>();

                foreach (var item in points)
                {
                    var temp = new PointDetailVM()
                    {
                        PointId = item.PointId.ToString(),
                        NameLocation = item.NameLocation,
                        Address = item.Address,
                        CityName = _cityReporitory.GetNameCityById(item.CityId),
                        Lat = item.Lat,
                        Lng = item.Lng
                    };

                    result.Add(temp);
                }
                return result;

            }
            return null;
        }

        public void UpdatePoint(PointDetailVM pointVM, Guid id)
        {
            var point = _mapper.Map<Point>(pointVM);
            var city = _cityReporitory.GetCityDetailbyName(pointVM.CityName);
            if (city != null)
            {
                point.CityId = Guid.Parse(city.CityId);
                Console.WriteLine(point);
                _points.ReplaceOne(p => p.PointId.Equals(id), point);
            }

        }
    }
}
