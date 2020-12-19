using System;
using System.Threading.Tasks;

namespace AirQualityService.Services.Interface
{
    public interface IMqttClientPublishMesseage
    {
        public Task PublishAsync(string topic, string payload, string contentType);
    }
}
