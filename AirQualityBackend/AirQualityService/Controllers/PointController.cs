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
        public ActionResult DeletePointById(int id)
        {
            if (ModelState.IsValid)
            {
                _pointRepository.DeletePointById(id);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        public ActionResult UpdatePoint([FromBody] PointDetailVM point, int id)
        {
            if (ModelState.IsValid)
            {
                _pointRepository.UpdatePoint(point, id);
                return Ok();
            }
            return BadRequest();
        }

    }
}