using System;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirQualityService.Data;
using AirQualityService.Helpers.@interface;
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
        private IAQIHelper _AQIHeper;
        private static DateTime dateNow;
        public MqttClientService(AirQualityContext context, IMqttClientOptions options, IAQIHelper aQIHelper)
        {
            _AQIHeper = aQIHelper;
            _context = context;
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
            ConfigureMqttClient();

            dateNow = DateTime.Now;
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
            Console.WriteLine(json.ToString());
            return;
            var now = DateTime.Now;
            //  var nowLocal = DateTime.SpecifyKind(now, DateTimeKind.Local);
            var nowLocal = now;
            DateTime dateTime = new DateTime(nowLocal.Year, nowLocal.Month, nowLocal.Day, nowLocal.Hour, 0, 0);

            //DateTime dateTime1 = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, 0, 0);
            // dateNow = dateNow.AddHours(1);
            AirQuality air = new AirQuality()
            {
                PointId = Guid.Parse((string)jObject["id"]),
                DateTime = dateTime.AddHours(-1),
                Humidity = (float)jObject["humidity"],
                Temperature = (float)jObject["temperature"],
                O3 = (float)jObject["O3"],
                CO = (float)jObject["CO"],
                NO2 = (float)jObject["NO2"],
                SO2 = (float)jObject["SO2"],
                PM2_5 = (int)jObject["pm2_5"],
                PM10_0 = (int)jObject["pm10_0"],

            };
            // Console.WriteLine(JObject.FromObject(air).ToString());

            air.AQIInHour = _AQIHeper.GetAQIInHour(air);
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
            try
            {

                var tsk = mqttClient.ReconnectAsync();
                tsk.Start();
                if (mqttClient.IsConnected)
                {
                    mqttClient.UseDisconnectedHandler(x =>
                    {
                        Console.WriteLine("hbhbg" + x.Exception.Message);
                    });
                }

            }
            catch (Exception e)
            {

            }


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


            try
            {
                if (!mqttClient.IsConnected)
                {
                    await mqttClient.ReconnectAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Reconnection error: " + ex);
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
                                                                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);


                var result = await mqttClient.PublishAsync(messageBuilder.Build());

                Console.WriteLine(result.ReasonString);

            }
        }
    }
}
