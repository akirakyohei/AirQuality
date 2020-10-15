using System;
using AirQualityService.Interface.Services;

namespace AirQualityService.Services
{
    public class MqttClientServiceProvider
    {
        public readonly IMqttClientService mqttClientService;

        public MqttClientServiceProvider(IMqttClientService mqttClientService)
        {
            this.mqttClientService = mqttClientService;
        }
    }
}
