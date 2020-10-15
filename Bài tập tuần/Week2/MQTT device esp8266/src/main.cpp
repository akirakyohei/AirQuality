#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ArduinoJson.h>
#include "PubSubClient.h"

const char *ssid = "Thu Ha";
const char *password = "20012001";
const char *mqtt_broker = "broker.mqttdashboard.com";
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);
void callback(char *topic, byte *payload, unsigned int length);
String generateJson(int id, int packet_no, int temperature, int humidity, int tds, float pH);
void setup()
{
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to the WiFi network");

  client.setServer(mqtt_broker, mqtt_port);
  client.setCallback(callback);
  while (!client.connected())
  {
    Serial.println("Connecting to public emqx mqtt broker.....");
    if (client.connect("espClient"))
    {
      Serial.println("Public emqx mqtt broker connected");
    }
    else
    {
      Serial.print("failed with state ");
      Serial.println(client.state());
      delay(1000);
    }
  }

  client.subscribe("testtopic/airquality");
}

void loop()
{
  String payload = generateJson(2, 12, 4, 60, 34, 7.0);
  client.publish("testtopic/airquality", payload.c_str());
  client.loop();
  delay(5000);
}

void callback(char *topic, byte *payload, unsigned int length)
{
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
  Serial.print("Message : ");
  for (unsigned int i = 0; i < length; ++i)
  {
    Serial.print((char)payload[i]);
  }
  Serial.println();
  Serial.println("_____________________");
}
String generateJson(int id, int packet_no, int temperature, int humidity, int tds, float pH)
{
  StaticJsonDocument<1024> doc;
  doc["id"] = id;
  doc["packet_no"] = packet_no;
  doc["temperature"] = temperature;
  doc["humidity"] = humidity;
  doc["tds"] = tds;
  doc["pH"] = pH;

  String payload = "";
  serializeJson(doc, payload);
  return payload;
}