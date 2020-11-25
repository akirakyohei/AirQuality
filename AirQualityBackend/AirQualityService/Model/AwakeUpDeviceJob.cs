using System;
using System.Threading.Tasks;

using AirQualityService.Services;
using AirQualityService.Setting;
using Quartz;

namespace AirQualityService.Model
{
    [DisallowConcurrentExecution]
    public class AwakeUpDeviceJob : IJob
    {
        private MqttClientService _mqttClientService;

        public AwakeUpDeviceJob(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Awake up device");

            string payload = "{\"command\":\"awakeup\"}";
            await _mqttClientService.PublishAsync("iot-2/type/NodeMcu/id/Broker_AirQuality/cmd/command/fmt/json", payload, "json");

        }
    }
}
