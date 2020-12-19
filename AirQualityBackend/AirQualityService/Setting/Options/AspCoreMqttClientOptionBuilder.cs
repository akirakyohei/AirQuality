using System;
using MQTTnet.Client.Options;

namespace AirQualityService.Setting.Options
{
    public class AspCoreMqttClientOptionBuilder : MqttClientOptionsBuilder
    {
        public IServiceProvider serviceProvider { get; }

        public AspCoreMqttClientOptionBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
