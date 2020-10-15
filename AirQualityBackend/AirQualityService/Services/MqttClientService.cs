using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirQualityService.Data;
using AirQualityService.Interface.Services;
using AirQualityService.model;
using AirQualityService.Setting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirQualityService.Services
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;
        private readonly AirQualityContext _context;
        public MqttClientService(AirQualityContext context, IMqttClientOptions options)
        {
            _context = context;
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
            ConfigureMqttClient();
        }

        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }


        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Console.WriteLine("handing message!");


            var payload = eventArgs.ApplicationMessage.Payload;
            var json = Encoding.UTF8.GetString(payload);

            JObject jObject = JObject.Parse(json);

            AirQuality air = new AirQuality()
            {
                PointId = (int)jObject["id"],
                DateTime = DateTime.Now,
                Humidity = (float)jObject["humidity"],
                Temperature = (float)jObject["temperature"],
                PM1_0 = (int)jObject["pm1_0"],
                PM2_5 = (int)jObject["pm2_5"],
                PM10_0 = (int)jObject["pm10_0"],
                PPM = (int)jObject["PPM"]

            };

            Console.WriteLine(JObject.FromObject(air).ToString());
            await _context.AirQualities.InsertOneAsync(air);


        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("connected");

            string payload = "{\"ID\":\"12\"}";
            await PublishAsync(MQTTClientSettings.instance.TopicPublish, payload, "json");

            var result = await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                                                        .WithTopic(MQTTClientSettings.instance.TopicSubscribe)
                                                        .Build());
            Console.WriteLine(result.Items);
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {

            mqttClient.UseDisconnectedHandler(x =>
            {
                Console.WriteLine("hbhbg" + x.Exception.Message);
            });

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await mqttClient.ConnectAsync(options);
                Console.WriteLine(result.ResultCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex);
            }



            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDiconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }

        public async Task PublishAsync(string topic, string payload, string contentType)
        {
            if (mqttClient.IsConnected)
            {
                MqttApplicationMessageBuilder messageBuilder = new MqttApplicationMessageBuilder()
                                                                    .WithContentType(contentType)
                                                                    .WithPayload(payload)
                                                                    .WithTopic(topic)
                                                                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);


                var result = await mqttClient.PublishAsync(messageBuilder.Build());
                Console.WriteLine(result.ReasonString);
            }
        }
    }
}
