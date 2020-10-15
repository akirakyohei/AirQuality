using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityRepository _airQualityRepository;

        public AirQualityController(IAirQualityRepository airQualityRepository)
        {
            _airQualityRepository = airQualityRepository;
        }


        [HttpGet("list/{pointId}/{dateTimeFrom}/{limmit}")]
        public ActionResult<List<AirQualityVM>> GetAirQualitiesByPointId(int pointId, DateTime dateTimeFrom, int limit)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualitiesByPointId(pointId, dateTimeFrom, limit);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("{pointId}/{dateTimeFrom}")]
        public ActionResult<AirQualityVM> GetAirQualityByPointId(int pointId, DateTime dateTimeFrom)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualityByPointId(pointId, dateTimeFrom);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("current/{pointId}")]
        public ActionResult<AirQualityVM> GetAirQualityCurrentByPointId(int pointId)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualityCurrentByPointId(pointId);
                return Ok(result);
            }
            return BadRequest();
        }

    }
}