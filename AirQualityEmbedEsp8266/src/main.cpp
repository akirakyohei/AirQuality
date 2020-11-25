#include <Arduino.h>
#include <Wire.h>
#include <string.h>
#include <ESP8266WiFi.h>
#include <ESP8266mDNS.h>
#include <ArduinoJson.h>
#include <WiFiClientSecure.h>
#include <ESP8266WebServer.h>
#include <FS.h>
#include <AutoConnect.h>
#include <PubSubClient.h>
#include <config.h>
// const char* username="admin";
// const char* password="admin";

#define I2C_SLAVE 6
#define I2C_OWN 8
#define CMD_SEND_DATA_MODE 0

#define ORG "f4vt93"
#define DEVICE_TYPE "NodeMcu"

// #define DEVICE_ID "esp-1"
// #define TOKEN "xKc5WxqzXa)Bp3oLH&"

const char *eventTopic = "iot-2/evt/status/fmt/json";
const char *cmdTopic = "iot-2/cmd/inc_brightness/fmt/json";
const char fingerprint[] = "16:51:E3:C2:67:C6:AD:23:9C:1A:70:5C:22:A3:B8:C1:7B:7C:A6:1D";
char server[] = ORG ".messaging.internetofthings.ibmcloud.com";
char authMethod[] = "use-token-auth";
bool stateMQTT=false;

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
void mqttConnect();
void mqttDisconnect();

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

void handleNotFound(){
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
  bool state = saveConfiguarationAuth(auth_config);
  String mess="";
  if(state){
    mess="{\"success\" : true }";
  }else
  {
      mess="{\"success\" : false }";
  }
  
  Server.send(200,"application/json",mess);
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
   bool state =  saveConfiguarationMQTT(mqtt_config);
    String mess="";
  if(state){
    mess="{\"success\" : true }";
  }else
  {
      mess="{\"success\" : false }";
  }
  
  Server.send(200,"application/json",mess);
}

void resetEsp8266(){
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
      auth_config.username="admin";
      auth_config.password="admin";
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
      DeserializationError error = deserializeJson(doc,file);
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
        Server.send(200,"application/json",output);
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
      serializeJson(doc,Serial);
      if (error)
      {
        Serial.println("failed to read file");
      }
    }
  }
  file.close();
  //
  SPIFFS.remove(path);
  File file1 = SPIFFS.open(path,"w");
  if(!file1){
    Serial.println("error opening file for writing!");
  }
  Serial.println(config.password);
  Serial.println(config.username);
  doc["username"] = config.username;
  doc["password"] = config.password;
  
   serializeJson(doc,Serial);
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
       serializeJson(doc,Serial);
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
   serializeJson(doc,Serial);
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

void callback(char *topic, byte *payload, unsigned int lenght)
{
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("]");
  String cmd = "";
  for (unsigned int i = 0; i < lenght; i++)
  {
    Serial.print((char)payload[i]);
    cmd = cmd + (char)payload[i];
  }
  if (cmd.equals("GET_SAMPLE"))
  {
    Serial.println("GET_SAMPLE");
    Wire.beginTransmission(I2C_SLAVE);
    Wire.write(CMD_SEND_DATA_MODE);
    Wire.endTransmission();
  }
  Serial.println();
}

PubSubClient client(server, 8883, callback, wifiClient);

void mqttConnect()
{
  loadMQTT();
  char buffClientId[] = "d:" ORG ":" DEVICE_TYPE ":";
  char *clientId = strcat(buffClientId, (char *)(mqtt_config.device_id.c_str()));
  Serial.println(clientId);
  int count_try_connect = 0;
  if (client.state() == MQTT_CONNECTED)
  {
    Serial.println("mqtt connected before.");
    return;
  }

  while (!client.connected())
  {
    if (client.connect(clientId, authMethod, (char *)(mqtt_config.token.c_str())))
    {
      Serial.println("MQTT connected.");
      stateMQTT=true;
    }
    else
    {
      Serial.print("MQTT connect fail , rf=");
      Serial.println(client.state());
      count_try_connect++;
      if (count_try_connect > 1)
      {
        Serial.println("Can't connect mqtt");
        return;
      }
    }
    delay(500);
  }
  if (client.subscribe(cmdTopic))
  {
    Serial.println("Subscribe to response OK");
  }
  else
  {
    Serial.println("Subscribe to response FAIL");
  }
}



// i2c
void recieveEvent(int args)
{
  while (Wire.available())
  {

    String data = Wire.readString();
    Serial.print("data recieved from i2c");
    Serial.println(data);
    if (client.state() != MQTT_CONNECTED)
      mqttConnect();

    if (client.publish(eventTopic, (char *)data.c_str()))
    {
      Serial.println("Publish OK");
    }
    else
    {
      Serial.println("Publish FAIL");
    }
  }
}

// setup
void setup()
{
  delay(6000);
  Serial.begin(9600);
  Serial.println();
  Wire.begin(I2C_OWN);
  Wire.onReceive(recieveEvent);
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

  //  loadConfig();
  loadAuth();

  // web server static
  Server.serveStatic("/css", SPIFFS, "/css");
  Server.serveStatic("/js", SPIFFS, "/js");
  Server.serveStatic("/home.html", SPIFFS, "/home.html");
  // web server route
  Server.on("/", rootPage);
  Server.on("/logout", logout);
  Server.on("/updateAccount",HTTP_POST,updateAccount);
  Server.on("/updateMQTT",HTTP_POST,updateMQTT);
  Server.on("/config.json",GetConfig);
  Server.on("/reset",resetEsp8266);
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
  mqttConnect();

  Serial.println("looping");
}

void loop()
{
  Portal.handleClient();
  Portal.handleRequest();
  MDNS.update();
  if (!client.loop()&&stateMQTT)
  {
    mqttConnect();
  }
}