using System;
using System.Collections.Generic;
using AirQualityService.Model;
using AirQualityService.ViewModels;

namespace AirQualityService.Data.Interface
{
    public interface IPointRepository
    {
        public List<PointVM> GetNamePoints();
        public PointDetailVM GetDetailPointById(Guid id);
        public List<PointAirQualityVM> GetPointAirQualities(Guid cityId);
        public List<PointDetailVM> GetPoints();
        public List<PointAirQualityVM> GetPointAirQualities();
        public List<Guid> GetListPointIds();
        public Guid GetCityIdComposePoint(Guid pointId);
        public Point AddPoint(PointDetailVM point);
        public void DeletePointById(Guid id);
        public void UpdatePoint(PointDetailVM point, Guid id);
    }
}
