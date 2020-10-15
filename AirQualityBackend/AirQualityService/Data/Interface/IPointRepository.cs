using System;
using System.Collections.Generic;
using AirQualityService.Model;
using AirQualityService.ViewModels;

namespace AirQualityService.Data.Interface
{
    public interface IPointRepository
    {
        public List<PointVM> GetNamePoints();
        public PointDetailVM GetDetailPointById(int id);
        public Point AddPoint(PointDetailVM point);
        public void DeletePointById(int id);
        public void UpdatePoint(PointDetailVM point, int id);
    }
}
