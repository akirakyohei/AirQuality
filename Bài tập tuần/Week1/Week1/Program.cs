using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Week1
{

    class Program
    {
        static void Main(string[] args)
        {
            GetInfoUser("8a0fc66a61a959f6", "a652d57094b7590b0dea115b156c07098abdea87", "P22498244182551944");
        }
        public static void GetInfoUser(string deviceId, string qrCodeId, string qrCodeValue)
        {


            string uri = "api/AccessControl/GetUserInfor";


            Dictionary<string, string> paBody = new Dictionary<string, string>();
            paBody.Add("deviceId", deviceId);
            paBody.Add("qrCodeId", qrCodeId);
            paBody.Add("qrCodeValue", qrCodeValue);


            var json = System.Text.Json.JsonSerializer.Serialize(paBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");


            using (HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(20) })
            {
                try
                {

                    client.BaseAddress = new Uri("http://203.171.20.94:8012/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                    requestMessage.Content = data;
                    var response = client.SendAsync(requestMessage).Result;


                    var result = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        JObject jObject = JObject.Parse(result);
                        Dictionary<string, string> info = jObject.ToObject<Dictionary<string, string>>();

                        Console.WriteLine("Get info device:");
                        foreach (var key in info.Keys)
                        {

                            Console.WriteLine(key + " : " + info[key]);
                        }
                    }
                    catch (JsonReaderException ex)
                    {
                        Console.WriteLine("Can't get json data response");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bad Request!");
                    Console.WriteLine("Error: " + ex);
                }


            }





        }


    }
}
