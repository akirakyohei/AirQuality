using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data;
using AirQualityService.Data.Interface;
using AirQualityService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityReporitory _cityRepository;

        public CityController(ICityReporitory cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet("listName")]
        public ActionResult<List<CityVM>> GetNameCities()
        {
            var result = _cityRepository.GetNameCities();

            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpGet("list")]
        public ActionResult<List<CityDetailVM>> GetCities()
        {
            var result = _cityRepository.GetCities();

            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpGet("{nameCity}", Name = "GetCityDetail")]
        public ActionResult<CityDetailVM> GetCityDetailbyName(string nameCity)
        {
            if (ModelState.IsValid)
            {
                var result = _cityRepository.GetCityDetailbyName(nameCity);

                if (result == null) return NoContent();
                return Ok(result);
            }
            return BadRequest();
        }


    }
}