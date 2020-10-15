#include <Arduino.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266WebServer.h>
#include <ESP8266HTTPClient.h>

#include <MQ135.h>
#include "DHT.h"
#include "PMS.h"


// bien
PMS pms(Serial1);
PMS::DATA data;


#define DHTPIN 2
#define DHTTYPE DHT22
DHT dht(DHTPIN, DHTTYPE);

#define PIN_MQ135 A0
MQ135 mq135_sensor = MQ135(PIN_MQ135);

String apiKey = "29483457t745bf8954958";
const char* ssid = "IoTVietNam";
const char* password = "nhom11@123";
const char* host = "https://localhost";
const int port = 80;
const char* path = "api/update";
const char* IdDevice="233452";




// ham
void setup()
{
  Serial.begin(9600);
  Serial1.begin(9600);
  pms.passiveMode();

  dht.begin();
 WiFi.begin(ssid,password);
 while(WiFi.status() != WL_CONNECTED){
   delay(500);
   Serial.println(".");
 }
 Serial.println("");
 Serial.println("Wifi Connected");
 Serial.println(WiFi.localIP());
 
}

void loop()
{
  //doc gia tri nhiet do, do am
  float humidity = dht.readHumidity();
  float t = dht.readTemperature();
  if (isnan(humidity) || isnan(t))
  {
    return;
  }
  float temperature = dht.computeHeatIndex(t, humidity, false);

  Serial.print("Temperature: ");
  Serial.print(temperature);
  Serial.println("˚C");
  Serial.print("Humidity: ");
  Serial.print(humidity);
  Serial.println("%");

  //doc gia tri cam bien khong khi
  float correctedPPM = mq135_sensor.getCorrectedPPM(temperature, humidity);
  Serial.print("PPM: ");
  Serial.print(correctedPPM);
  Serial.println("ppm");

  //doc gia tri bui min

  pms.wakeUp();
  delay(3000);
  pms.requestRead();

  if (pms.readUntil(data))
  {
    Serial1.print("PM 1.0 (ug/m3): ");
    Serial1.println(data.PM_AE_UG_1_0);

    Serial1.print("PM 2.5 (ug/m3): ");
    Serial1.println(data.PM_AE_UG_2_5);

    Serial1.print("PM 10.0 (ug/m3): ");
    Serial1.println(data.PM_AE_UG_10_0);
  }
  else
  {
    Serial1.println("No data.");
  }
  pms.sleep();
  
 

  delay(60000);
  
  
  BearSSL::WiFiClientSecure client;
  client.setInsecure();
  HTTPClient http;
  http.addHeader("Content-Type","application/json");

 StaticJsonDocument<200> doc;

 doc["temperature"]= temperature;
 doc["humidity"]=humidity;
 doc["PPM"]=correctedPPM;
 doc["pm1_0"]=data.PM_AE_UG_1_0;
 doc["pm2_5"]=data.PM_AE_UG_2_5;
 doc["pm10_0"]=data.PM_AE_UG_10_0;

 String payload="";
 serializeJson(doc,payload);
 if(http.begin(client,host,port,path,true)){
  int status= http.POST(payload);
  if(status == 200){
    Serial.println("Success update data!");
  }else{
    Serial.println("Error update data!");
  }
 }else{
   Serial.println("Connect url faid!");
 }

Serial.println("---------------------------");

}