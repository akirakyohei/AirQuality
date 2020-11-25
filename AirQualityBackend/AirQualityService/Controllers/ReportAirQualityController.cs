using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportAirQualityController : ControllerBase
    {
        private readonly IReportAirQualityInDayRepository reportAirQuality;
        private readonly IPointRepository _pointRepository;

        public ReportAirQualityController(IReportAirQualityInDayRepository reportAirQuality, IPointRepository pointRepository)
        {
            this.reportAirQuality = reportAirQuality;
            _pointRepository = pointRepository;
        }

        [HttpGet("aqi/{pointId}/{datetime}")]
        public ActionResult GetAQI_In_Day(Guid pointId, DateTime date)
        {
            if (ModelState.IsValid)
            {
                var result = reportAirQuality.GetAQI(pointId, date);

                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet("aqi/point")]
        public ActionResult GetReportPointAndAqiInCity([FromQuery] string cityId)
        {
            if (ModelState.IsValid)
            {
                var result = _pointRepository.GetPointAirQualities(Guid.Parse(cityId));
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("aqi/point/list")]
        public ActionResult GetReportPointAndAqiInCity()
        {
            if (ModelState.IsValid)
            {
                var result = _pointRepository.GetPointAirQualities();
                return Ok(result);
            }
            return BadRequest();
        }
    }
}