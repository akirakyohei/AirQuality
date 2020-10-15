using System;
namespace AirQualityService.Setting
{

    public class MQTTClientSettings
    {

        public static MQTTClientSettings instance;

        public string OrganizationID { get; set; }

        public string DeviceType { get; set; }

        public string DeviceID { get; set; }

        public string AuthenticationMethod { get; set; }

        public string AuthenticationToken { get; set; }

        public string TopicSubscribe { get; set; }

        public string TopicPublish { get; set; }

        public int Port { get; set; }

    }
}
