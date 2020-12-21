using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AirQualityService.Helpers.@interface;
using AirQualityService.Setting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AirQualityService.Helpers
{
    public class IBMClientHelper : IIBMClientHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        //public IBMClientHelper()
        //{
        //    settings = MQTTClientSettings.instance;
        //    //client = new IBMWIoTP.ApiClient("a-f4vt93-9dedycmmak", "TU5)ARZ7qfCwFCM&(x");
        //}
        public IBMClientHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> registerDevice(string deviceId)
        {
            var content = "[{\"typeId\": \"NodeMcu\",\"deviceId\": \"" + deviceId + "\"}]";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://f4vt93.internetofthings.ibmcloud.com/api/v0002/bulk/devices/add")
            {

                Content = new StringContent(content, Encoding.UTF8, "application/json"),
            };

            request.Headers.Add("Authorization", "Basic YS1mNHZ0OTMtOWRlZHljbW1hazpUVTUpQVJaN3FmQ3dGQ00mKHg=");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return responseString;
            }
            Console.WriteLine("njj");
            return "";
        }

        public async Task<string> removeDevice(string deviceId)
        {
            var content = "[{\"typeId\": \"NodeMcu\",\"deviceId\": \"" + deviceId + "\"}]";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://f4vt93.internetofthings.ibmcloud.com/api/v0002/bulk/devices/remove")
            {

                Content = new StringContent(content, Encoding.UTF8, "application/json"),
            };

            request.Headers.Add("Authorization", "Basic YS1mNHZ0OTMtOWRlZHljbW1hazpUVTUpQVJaN3FmQ3dGQ00mKHg=");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return responseString;
            }

            return "";
        }

        public async Task<string> LogConnection(string deviceId)
        {
            var param = "?typeId=NodeMcu&deviceId=" + deviceId;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://f4vt93.internetofthings.ibmcloud.com/api/v0002/logs/connection" + param)
            {

            };

            request.Headers.Add("Authorization", "Basic YS1mNHZ0OTMtOWRlZHljbW1hazpUVTUpQVJaN3FmQ3dGQ00mKHg=");


            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return responseString;
            }

            return "";
        }

        public async Task<string> LogDiagDevice(string deviceId)
        {
            var url = "https://f4vt93.internetofthings.ibmcloud.com/api/v0002/device/types/NodeMcu/devices/" + deviceId + "/diag/logs";
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {

            };

            request.Headers.Add("Authorization", "Basic YS1mNHZ0OTMtOWRlZHljbW1hazpUVTUpQVJaN3FmQ3dGQ00mKHg=");


            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return responseString;
            }

            return "";
        }
    }
}
