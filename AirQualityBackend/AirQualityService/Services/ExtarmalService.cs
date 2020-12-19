using System;
using AirQualityService.Interface.Services;

namespace AirQualityService.Services
{
    public class ExtarmalService
    {
        private readonly IMqttClientService mqttClientService;
        public ExtarmalService(MqttClientServiceProvider provider)
        {
            mqttClientService = provider.mqttClientService;
        }
    }
}
