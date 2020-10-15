using System;
using System.Text;
using AirQualityService.Services;
using AirQualityService.Setting;
using AirQualityService.Setting.Options;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Options;

namespace AirQualityService.Helpers
{
    public static class ServiceCollectionExtension
    {
        [Obsolete]
        public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services)
        {
            services.AddMqttClientServiceWithConfig(aspCoreMqttClientOptionBuilder =>
            {
                var settings = MQTTClientSettings.instance;
                // string mqttClientId = "d:" + settings.OrganizationID + ":" + settings.DeviceType + ":" + settings.DeviceID;
                string mqttClientId = "a:f4vt93:dfjkvnjvjds";
                string mqttServer = settings.OrganizationID + ".messaging.internetofthings.ibmcloud.com";
                string mqttMethod = settings.AuthenticationMethod;
                string mqttToken = settings.AuthenticationToken;
                int mqttPort = settings.Port;
                //string mqttKey = "a-f4vt93-ghmlhla6fp";
                //string mqttToken = "-u35gjs-fuNvI*VIOx";


                aspCoreMqttClientOptionBuilder
                 .WithCredentials(mqttMethod, mqttToken)
               .WithClientId(mqttClientId)
               .WithTls()
               .WithKeepAlivePeriod(TimeSpan.MaxValue)
                .WithKeepAliveSendInterval(TimeSpan.FromSeconds(300))
               .WithCommunicationTimeout(TimeSpan.FromSeconds(300))
               .WithTcpServer(mqttServer, mqttPort)
                .WithWebSocketServer(mqttServer);
            });
            return services;
        }
        private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<AspCoreMqttClientOptionBuilder> configure)
        {
            services.AddSingleton<IMqttClientOptions>(servicesProvider =>
            {
                var optionBuilder = new AspCoreMqttClientOptionBuilder(servicesProvider);
                configure(optionBuilder);
                return optionBuilder.Build();
            });
            services.AddSingleton<MqttClientService>();
            services.AddSingleton<IHostedService>(serviceProvider =>
            {
                return serviceProvider.GetService<MqttClientService>();
            });
            services.AddSingleton<MqttClientServiceProvider>(serviceProvider =>
            {
                var mqttClientService = serviceProvider.GetService<MqttClientService>();
                var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
                return mqttClientServiceProvider;
            });
            return services;
        }

    }
}
