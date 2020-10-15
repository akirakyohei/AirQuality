using System;
using System.Threading.Tasks;
using AirQualityService.Services.Interface;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace AirQualityService.Interface.Services
{
    public interface IMqttClientService : IHostedService,
                                        IMqttClientConnectedHandler,
                                        IMqttClientDisconnectedHandler,
                                        IMqttApplicationMessageReceivedHandler,
                                        IMqttClientPublishMesseage



    {

    }


}
