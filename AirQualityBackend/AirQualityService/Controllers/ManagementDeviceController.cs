using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.Helpers.@interface;
using AirQualityService.Model;
using AirQualityService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirQualityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ManagementDeviceController : ControllerBase
    {
        private readonly IIBMClientHelper _IBMClientHelper;
        private readonly IPointRepository _pointRepository;

        public ManagementDeviceController(IIBMClientHelper iBMClientHelper, IPointRepository pointRepository)
        {
            _IBMClientHelper = iBMClientHelper;
            _pointRepository = pointRepository;
        }

        [HttpPost("register"), Authorize(Roles = "Manager")]
        public async Task<ActionResult> registerDevice([FromBody] PointDetailVM point)
        {
            var result = _pointRepository.AddPoint(point);
            if (result.PointId != null)
            {

                var ibmDevice = await _IBMClientHelper.registerDevice(result.PointId.ToString());
                return Ok(new { point = result, ibmResult = ibmDevice });
            }

            return BadRequest();
        }

        [HttpGet("listAll"), Authorize(Roles = "Manager")]
        public ActionResult getAllDevice()
        {
            var result = _pointRepository.GetPoints();
            return Ok(result);
        }

        [HttpDelete("remove"), Authorize(Roles = "Manager")]
        public async Task<ActionResult> removeDeviceAsync([FromQuery] string deviceId)
        {
            _pointRepository.DeletePointById(new Guid(deviceId));
            var resultIbm = await _IBMClientHelper.removeDevice(deviceId);
            return Ok(new { ibmMess = resultIbm });
        }

        [HttpPut("update"), Authorize(Roles = "Manager")]
        public ActionResult updateDevice([FromBody] PointDetailVM point, string id)
        {
            _pointRepository.UpdatePoint(point, new Guid(id));
            return Ok();
        }

        [HttpGet("Log/Connection"), Authorize(Roles = "Manager")]
        public ActionResult LogConnection([FromQuery] string deviceId)
        {

            var logs = _IBMClientHelper.LogConnection(deviceId);
            return Ok(new { log = logs });

        }
        [HttpGet("Log/Dialog"), Authorize(Roles = "Manager")]
        public ActionResult LogDialog([FromQuery] string deviceId)
        {
            var logs = _IBMClientHelper.LogDiagDevice(deviceId);
            return Ok(new { log = logs });
        }
    }
}