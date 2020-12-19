using System;
using System.Threading.Tasks;
using AirQualityService.Data.Interface;
using AirQualityService.Services;
using AirQualityService.Setting;
using AirQualityService.Setting.Options;
using Quartz;

namespace AirQualityService.Model
{
    [DisallowConcurrentExecution]
    public class AwakeUpDeviceJob : IJob
    {
        private MqttClientService _mqttClientService;
        private IPointRepository _pointRepository;

        public AwakeUpDeviceJob(MqttClientService mqttClientService, IPointRepository pointRepository)
        {
            _mqttClientService = mqttClientService;
            _pointRepository = pointRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("Awake up device");

            var pointList = _pointRepository.GetListPointIds();

            pointList.Add(new Guid("d06aa5df-85b3-4d52-9b2d-ffc23139031b"));
            //pointList.Add(new Guid("esp-1"));
            string payload = "{\"cmd\": \"" + CommandDevice.GET_SAMPLE.ToString() + "\"}";


            foreach (var item in pointList)
            {
                Console.WriteLine("Wakeup device " + item.ToString());

                await _mqttClientService.PublishAsync("iot-2/type/NodeMcu/id/" + item.ToString() + "/cmd/command/fmt/json", payload, "json");

            }

        }
    }
}
