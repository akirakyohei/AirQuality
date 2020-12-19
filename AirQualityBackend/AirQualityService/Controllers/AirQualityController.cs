using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirQualityController : ControllerBase
    {
        private readonly IAirQualityRepository _airQualityRepository;
        private readonly IMapper _mapper;

        public AirQualityController(IAirQualityRepository airQualityRepository, IMapper mapper)
        {
            _airQualityRepository = airQualityRepository;
            _mapper = mapper;
        }

        [HttpGet("list/{pointId}/{dateTimeFrom}/{limmit}")]
        public ActionResult<List<AirQualityVM>> GetAirQualitiesByPointId(Guid pointId, DateTime dateTimeFrom, int limmit)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualitiesByPointId(pointId, dateTimeFrom, limmit);
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("listById")]
        public ActionResult<List<AirQualityVM>> GetAirQualitiesByPointId([FromQuery] Guid pointId)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualitiesByPointId(pointId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("{pointId}/{dateTimeFrom}")]
        public ActionResult<AirQualityVM> GetAirQualityByPointId(Guid pointId, DateTime dateTimeFrom)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualityByPointId(pointId, dateTimeFrom);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("current")]
        public ActionResult<List<AirQualityVM>> GetAirQualityCurrentByPointId([FromQuery] Guid pointId)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualityCurrentByPointId(pointId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("current/limmit")]
        public ActionResult<AirQualityVM> GetAirQualityCurrentLimitById([FromQuery] Guid pointId, [FromQuery] int limit)
        {
            if (ModelState.IsValid)
            {
                var result = _airQualityRepository.GetAirQualityNowLimit(pointId, limit);
                List<AirQualityVM> res = new List<AirQualityVM>();
                if (result != null)
                {


                    foreach (var item in result)
                    {
                        AirQualityVM temp = _mapper.Map<AirQualityVM>(item);
                        res.Add(temp);
                    }

                }
                return Ok(res);

            }
            return BadRequest();
        }

    }
}