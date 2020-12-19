using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.Model;
using AirQualityService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;

        public PointController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        [HttpGet("listName")]
        public ActionResult<List<PointVM>> GetNamePoints()
        {
            var result = _pointRepository.GetNamePoints();

            if (result == null) return NoContent();
            return Ok(result);
        }

        [HttpGet("list")]
        public ActionResult<List<string>> GetPointList()
        {
            var resultGuid = _pointRepository.GetListPointIds();
            List<string> result = new List<string>();
            foreach (var item in resultGuid)
            {
                result.Add(item.ToString());
            }
            return result;
        }

        [HttpGet("{id}")]
        public ActionResult<PointDetailVM> GetDetailPointById(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _pointRepository.GetNamePoints();

                if (result == null) return NoContent();
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost("add")]
        public ActionResult<Point> AddPoint([FromBody] PointDetailVM point)
        {
            if (ModelState.IsValid)
            {
                var result = _pointRepository.AddPoint(point);

                if (result == null) return NoContent();
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public ActionResult DeletePointById(Guid id)
        {
            if (ModelState.IsValid)
            {
                _pointRepository.DeletePointById(id);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        public ActionResult UpdatePoint([FromBody] PointDetailVM point, string id)
        {
            if (ModelState.IsValid)
            {
                _pointRepository.UpdatePoint(point, Guid.Parse(id));
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("pointInCity")]
        public ActionResult<string> GetCityIdbyPointId([FromQuery] string id)
        {
            Console.WriteLine(id.ToString());
            if (ModelState.IsValid)
            {
                var result = _pointRepository.GetCityIdComposePoint(Guid.Parse(id));
                if (result != Guid.Empty)
                {
                    Console.WriteLine(result.ToString());
                    return Ok(new { cityId = result.ToString() });
                }
                return Ok();
            }
            return BadRequest();
        }

    }
}