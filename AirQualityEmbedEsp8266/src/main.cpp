#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266mDNS.h>
#include <ArduinoJson.h>
#include <WiFiClientSecure.h>
#include <ESP8266WebServer.h>
#include <FS.h>
#include <AutoConnect.h>
#include <PubSubClient.h>
#include <config.h>
// #define BUFFER_LENGTH 1024
#include <Wire.h>
// const char* username="admin";
// const char* password="admin";

#define I2C_SLAVE 8
// #define I2C_OWN 8
#define CMD_SEND_DATA_MODE '0'

#define ORG "f4vt93"
#define DEVICE_TYPE "NodeMcu"

// #define DEVICE_ID "esp-1"
// #define TOKEN "xKc5WxqzXa)Bp3oLH&"

const char *eventTopic = "iot-2/evt/status/fmt/json";
const char *cmdTopic = "iot-2/cmd/command/fmt/json";
// iot - 2 / cmd / command_id / fmt / format_string
//const char fingerprint[] = "16:51:E3:C2:67:C6:AD:23:9C:1A:70:5C:22:A3:B8:C1:7B:7C:A6:1D";
const char fingerprint[] = "C7:49:0A:A4:85:B5:A5:82:F0:A7:F0:D0:1E:BD:88:28:7A:B3:2E:F7";

char server[] = ORG ".messaging.internetofthings.ibmcloud.com";
char authMethod[] = "use-token-auth";
bool stateMQTT = false;

// struct object
struct AuthConfig
{
  String username;
  String password;
};
struct MQTTConfig
{
  String device_id;
  String token;
};

//variable webserver
ESP8266WebServer Server(80);
AutoConnect Portal(Server);
//variable config
AutoConnectConfig config;
AuthConfig auth_config;
MQTTConfig mqtt_config;

bool saveConfiguarationAuth(const AuthConfig &config);
bool saveConfiguarationMQTT(const MQTTConfig &config);

// handle web server
void rootPage()
{
  if (!Server.authenticate((char *)(auth_config.username.c_str()), (char *)(auth_config.password.c_str())))
  {
    Server.requestAuthentication();
  }
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  File home = SPIFFS.open("/home.html", "r");
  Server.streamFile(home, "text/html");
  home.close();
}

void handleNotFound()
{
  if (Server.method() == HTTP_OPTIONS)
  {
    Server.sendHeader("Access-Control-Allow-Origin", "*");
    Server.sendHeader("Access-Control-Max-Age", "10000");
    Server.sendHeader("Access-Control-Allow-Methods", "PUT,POST,GET,OPTIONS");
    Server.sendHeader("Access-Control-Allow-Headers", "*");
    Server.send(204);
  }
  else
  {
    Server.send(404, "text/plain", "Page not found");
  }
}

void logout()
{
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  Server.send(401, "text/html", "logged out!");
}

void updateAccount()
{
  if (!Server.authenticate((char *)(auth_config.username.c_str()), (char *)(auth_config.password.c_str())))
  {
    Server.requestAuthentication();
  }
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  Serial.println("updateAccount");
  if (Server.hasArg("username"))
  {
    auth_config.username = Server.arg("username");
  }
  if (Server.hasArg("password"))
  {
    auth_config.password = Server.arg("password");
  }
  Serial.println(auth_config.username);
  Serial.println(auth_config.password);
  bool state = saveConfiguarationAuth(auth_config);
  String mess = "";
  if (state)
  {
    mess = "{\"success\" : true }";
  }
  else
  {
    mess = "{\"success\" : false }";
  }
  Serial.println(mess);
  Server.send(200, "application/json", mess.c_str());
}

void updateMQTT()
{
  if (!Server.authenticate((char *)(auth_config.username.c_str()), (char *)(auth_config.password.c_str())))
  {
    Server.requestAuthentication();
  }
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  if (Server.hasArg("deviceId"))
  {
    mqtt_config.device_id = Server.arg("deviceId");
  }
  if (Server.hasArg("token"))
  {
    mqtt_config.token = Server.arg("token");
  }
  bool state = saveConfiguarationMQTT(mqtt_config);
  String mess = "";
  if (state)
  {
    mess = "{\"success\" : true }";
  }
  else
  {
    mess = "{\"success\" : false }";
  }

  Server.send(200, "application/json", mess);
}

void resetEsp8266()
{
  if (!Server.authenticate((char *)(auth_config.username.c_str()), (char *)(auth_config.password.c_str())))
  {
    Server.requestAuthentication();
  }
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  Server.send(200);
  delay(1000);
  ESP.reset();
}

// config
void loadAuth()
{
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
      auth_config.username = "admin";
      auth_config.password = "admin";
    }

    else
    {

      StaticJsonDocument<1024> doc;
      DeserializationError error = deserializeJson(doc, file);
      if (error)
      {
        Serial.println("failed to read file");
      }
      else
      {
        JsonObject obj = doc.as<JsonObject>();
        auth_config.username = obj["username"].as<String>();
        auth_config.password = obj["password"].as<String>();
        Serial.println(auth_config.username);
        Serial.println(auth_config.password);
      }
    }
  }
  file.close();
}
void loadMQTT()
{
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
    }
    else
    {

      StaticJsonDocument<1024> doc;
      DeserializationError error = deserializeJson(doc, file);
      if (error)
      {
        Serial.println("failed to read file");
      }
      else
      {
        Serial.println("reading file");
        JsonObject obj = doc.as<JsonObject>();
        mqtt_config.device_id = obj["device_id"].as<String>();
        mqtt_config.token = obj["token"].as<String>();
        Serial.println(mqtt_config.device_id);
        Serial.println(mqtt_config.token);
        auth_config.username = obj["username"].as<String>();
        auth_config.password = obj["password"].as<String>();
        Serial.println(auth_config.username);
        Serial.println(auth_config.password);
      }
    }
  }
  file.close();
}
void loadConfig()
{
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
      auth_config.username = "admin";
      auth_config.password = "admin";
    }
    else
    {

      StaticJsonDocument<1024> doc;
      DeserializationError error = deserializeJson(doc, file);
      if (error)
      {
        Serial.println("failed to read file");
      }
      else
      {
        Serial.println("reading file");
        JsonObject obj = doc.as<JsonObject>();
        mqtt_config.device_id = obj["device_id"].as<String>();
        mqtt_config.token = obj["token"].as<String>();
        Serial.println(mqtt_config.device_id);
        Serial.println(mqtt_config.token);
        auth_config.username = obj["username"].as<String>();
        auth_config.password = obj["password"].as<String>();
        Serial.println(auth_config.username);
        Serial.println(auth_config.password);
      }
    }
  }
  file.close();
}

void GetConfig()
{
  if (!Server.authenticate((char *)(auth_config.username.c_str()), (char *)(auth_config.password.c_str())))
  {
    Server.requestAuthentication();
  }
  Server.sendHeader("Access-Control-Allow-Origin", "*");
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
    }
    else
    {

      StaticJsonDocument<1024> doc;
      DeserializationError error = deserializeJson(doc, file);
      if (error)
      {
        Serial.println("failed to read file");
      }
      else
      {
        JsonObject obj = doc.as<JsonObject>();
        mqtt_config.device_id = obj["device_id"].as<String>();
        mqtt_config.token = obj["token"].as<String>();
        Serial.println(mqtt_config.device_id);
        Serial.println(mqtt_config.token);
        auth_config.username = obj["username"].as<String>();
        auth_config.password = obj["password"].as<String>();
        Serial.println(auth_config.username);
        Serial.println(auth_config.password);
        String output;
        serializeJson(doc, output);
        Server.send(200, "application/json", output);
      }
    }
  }
  file.close();
}

bool saveConfiguarationAuth(const AuthConfig &config)
{
  bool state = false;
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  DynamicJsonDocument doc(1024);
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
    }
    else
    {

      DeserializationError error = deserializeJson(doc, file);
      serializeJson(doc, Serial);
      if (error)
      {
        Serial.println("failed to read file");
      }
    }
  }
  file.close();
  //
  SPIFFS.remove(path);
  File file1 = SPIFFS.open(path, "w");
  if (!file1)
  {
    Serial.println("error opening file for writing!");
  }
  Serial.println(config.password);
  Serial.println(config.username);
  doc["username"] = config.username;
  doc["password"] = config.password;

  serializeJson(doc, Serial);
  if (serializeJson(doc, file1) == 0)
  {
    Serial.println("Failed to write to file");
  }
  else
  {
    state = true;
  }
  file1.close();
  return state;
}

bool saveConfiguarationMQTT(const MQTTConfig &config)
{
  bool state = false;
  const char path[] = "/json/config.json";
  File file = SPIFFS.open(path, "r");
  DynamicJsonDocument doc(1024);
  if (!file)
  {
    Serial.println("path existe - No path exist");
  }
  else
  {
    size_t size = file.size();
    if (size == 0)
    {
      Serial.println("file empty");
    }
    else
    {
      DeserializationError error = deserializeJson(doc, file);
      serializeJson(doc, Serial);
      if (error)
      {
        Serial.println("failed to read file");
      }
    }
  }
  file.close();
  //
  SPIFFS.remove(path);
  File file1 = SPIFFS.open(path, "w");
  doc["device_id"] = config.device_id;
  doc["token"] = config.token;
  serializeJson(doc, Serial);
  if (serializeJson(doc, file1) == 0)
  {
    Serial.println("Failed to write to file");
  }
  else
  {
    state = true;
  }
  file1.close();
  return state;
}

// mqtt
WiFiClientSecure wifiClient;
void callback(char *topic, byte *payload, unsigned int lenght);
PubSubClient client(server, 8883, callback, wifiClient);
// WiFiClient wifiClient;

void get_sample()
{
  Wire.beginTransmission(8); /* begin with device address 8 */
  Wire.write("get_sample");  /* sends hello slave string */
  Wire.endTransmission();    /* stop transmitting */

  Wire.flush();
  Wire.requestFrom(8, 32);
  float arr[8];
  while (Wire.available() < 4)
  {
  }
  int i = 0;
  while (Wire.available())
  {
    // Serial.print("s");
    byte buff[4];
    Wire.readBytes(buff, 4);

    ((uint8_t *)&arr[i])[0] = buff[0];
    ((uint8_t *)&arr[i])[1] = buff[1];
    ((uint8_t *)&arr[i])[2] = buff[2];
    ((uint8_t *)&arr[i])[3] = buff[3];
    Serial.println(arr[i]);
    i++;
  }
  DynamicJsonDocument doc(1024);
  doc["id"] = mqtt_config.device_id;
  doc["temperature"] = arr[0];
  doc["humidity"] = arr[1];
  doc["o3"] = arr[2];
  doc["co"] = arr[3];
  doc["no2"] = arr[4];
  doc["so2"] = arr[5];
  doc["pm2_5"] = arr[6];
  doc["pm10"] = arr[7];

  String output;
  serializeJson(doc, output);
  Serial.println(output);
  client.publish(eventTopic, output.c_str());
}

void callback(char *topic, byte *payload, unsigned int lenght)
{
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("]");
  String cmd;
  for (unsigned int i = 0; i < lenght; i++)
  {
    Serial.print((char)payload[i]);
    cmd = cmd + (char)payload[i];
  }
  StaticJsonDocument<1024> doc;
  DeserializationError error = deserializeJson(doc, payload);
  if (error)
  {
    Serial.println("failed to read payload");
  }
  else
  {
    JsonObject obj = doc.as<JsonObject>();
    String cmd = obj["cmd"].as<String>();
    Serial.println(cmd);
    if (cmd.equals("GET_SAMPLE"))
    {
      Serial.println("GET_SAMPLE");
      get_sample();
    }
    Serial.println();
  }
}

// setup
void setup()
{
  delay(6000);
  Wire.begin(D1, D2);
  Serial.begin(9600);
  Serial.println();
  wifiClient.setFingerprint(fingerprint);

  // init SPIFFS
  if (!SPIFFS.begin())
  {
    Serial.println("SPIFFS mount failed");
  }
  else
  {
    Serial.println("SPIFFS  Mount successfull");
  }

  loadConfig();
  // loadAuth();
  // loadMQTT();

  // web server static
  Server.serveStatic("/css", SPIFFS, "/css");
  Server.serveStatic("/js", SPIFFS, "/js");
  // Server.serveStatic("/home.html", SPIFFS, "/home.html");
  // web server route
  Server.on("/", rootPage);
  Server.on("/logout", logout);
  Server.on("/updateAccount", HTTP_POST, updateAccount);
  Server.on("/updateMQTT", HTTP_POST, updateMQTT);
  Server.on("/config.json", GetConfig);
  Server.on("/reset", resetEsp8266);
  Server.onNotFound(handleNotFound);
  config.apid = "Device-sensor";
  config.psk = "dong1234";
  config.autoReconnect = true;
  config.menuItems = AC_MENUITEM_CONFIGNEW | AC_MENUITEM_DEVINFO | AC_MENUITEM_RESET | AC_MENUITEM_UPDATE | AC_MENUITEM_HOME;

  // AutoConnectCredential(0);

  // initial autoconnect
  Portal.config(config);
  if (Portal.begin())
  {
    Serial.println("WiFi connected: " + WiFi.localIP().toString());
    //  config DNS
    if (MDNS.begin("airquality"))
    {
      Serial.println("MDNS connected");
    }
    MDNS.addService("http", "tcp", 80);
  }

  // mqttConnect();
  char buffClientId[] = "d:" ORG ":" DEVICE_TYPE ":";
  char *clientId = strcat(buffClientId, (char *)(mqtt_config.device_id.c_str()));
  Serial.println(clientId);
  Serial.println(server);
  Serial.println((char *)(mqtt_config.token.c_str()));
  char *token = (char *)mqtt_config.token.c_str();
  if (client.connect(clientId, authMethod, token))
  {
    // client.publish("outTopic", "hello world");
    Serial.println("mqtt Connected.");
    stateMQTT = true;
    if (client.subscribe(cmdTopic))
    {
      Serial.println("Subscribe OK");
    };
  }
  else
  {
    Serial.println("mqtt don't Connect.");
    stateMQTT = false;
  }

  Serial.println("looping");
}

void loop()
{

  Portal.handleClient();
  Portal.handleRequest();
  MDNS.update();
  if (!client.loop() && stateMQTT)
  {
    char buffClientId[] = "d:" ORG ":" DEVICE_TYPE ":";
    char *clientId = strcat(buffClientId, (char *)(mqtt_config.device_id.c_str()));
    Serial.println(clientId);
    Serial.println(server);
    Serial.println((char *)(mqtt_config.token.c_str()));
    char *token = (char *)mqtt_config.token.c_str();
    if (client.connect(clientId, authMethod, token))
    {
      // client.publish("outTopic", "hello world");
      Serial.println("mqtt Connected.");
      stateMQTT = true;
      client.subscribe(cmdTopic);
    }
    else
    {
      Serial.println("mqtt don't Connect.");
    }
  }

  delay(10);
}
