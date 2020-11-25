#include <Arduino.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <MQ131.h>
#include <MQ_136.h>
#include <DHT.h>
#include <PMS.h>
// #include <MICS6814.h>
#include <MiCS6814-I2C.h>
#include <string.h>

// mode receiveCMD
#define CMD_SEND_DATA_MODE 0

// address master
#define I2C_MASTER 8
#define I2C_OWN 6
// ------------------------------------
#define DHTPIN 2       //temp , humidity
#define DHTTYPE DHT22  //
#define MQ131ANAPIN A0 //o3
#define MQ131DiGITALPIN 5
#define MQ136PIN A1 //so2
#define MICS6814_SDA A4 //support mics6814 i2c
#define MICS6814_SCL A5 //
//#define pms5003Tx RX //pm2.5 pm10
//#define pms5003Rx TX //
// #define SDA A4 //I2C
//#define SCL A5 //I2C
//GNDesp-GNDarduino //I2C
// #define COPIN A2  //co //not use i2c
// #define NO2PIN A3 //no2
// --------------------------------------
struct Air
{
  float temperature; //dht
  float humidity;    //dht
  float o3;          //mq131
  float so2;         //mq_136
  float co;          //mics6814-i2c
  float no2;         //mic6814-i2c
  float pm2_5;       //pms.h
  float pm10;        //pms.h
};

Air air;
//MICS6814 gas(COPIN, NO2PIN, 0);
MiCS6814 mics6814;
MQ_136 mqt136(MQ136PIN);
DHT dht(DHTPIN, DHTTYPE);
PMS pms(Serial);
PMS::DATA data;
int countSampleInHour;

void initAirIndexs()
{
  air.temperature = 0;
  air.humidity = 0;
  air.o3 = 0;
  air.so2 = 0;
  air.co = 0;
  air.no2 = 0;
  air.pm2_5 = 0;
  air.pm10 = 0;
  countSampleInHour = 0;
}
//https://how2electronics.com/iot-air-pollution-monitoring-esp8266/?fbclid=IwAR3mk5WB5F8ARIpocQDmNa7EVDJPxa3co2GIhWfpU0ibgRBg59YnGgfvGgE
void readEnv()
{
  air.humidity = dht.readHumidity();
  float t = dht.readTemperature();
  if (isnan(air.humidity) || isnan(t))
  {
    return;
  }
  air.temperature = dht.computeHeatIndex(t, air.humidity, false);
}
void readCO()
{
  // air.co = gas.measure(CO);
  air.co = mics6814.measureCO();
}
void readNO2()
{
  // air.co = gas.measure(NO2);
  air.no2 = mics6814.measureNO2();
}
void readO3()
{
  MQ131.setEnv(air.temperature, air.humidity);
  air.o3 = MQ131.getO3(UG_M3);
}

void readSO2()
{
  air.so2 = mqt136.getSo2();
}
void readPM()
{
  pms.wakeUp();
  delay(3000);
  pms.requestRead();
  if (pms.readUntil(data))
  {
    air.pm2_5 = data.PM_AE_UG_2_5;
    air.pm10 = data.PM_AE_UG_10_0;
  }
  pms.sleep();
}

void ReadAirQualityIndex()
{
  readEnv();
  readCO();
  readNO2();
  readO3();
  readSO2();
  readPM();
  countSampleInHour++;
}

Air GetAirQualityIndexInHour()
{
  Air airTemp;
  airTemp.temperature = air.temperature;
  airTemp.humidity = air.humidity;
  if (countSampleInHour > 0)
  {
    airTemp.o3 = air.o3 / countSampleInHour;
    airTemp.co = air.co / countSampleInHour;
    airTemp.so2 = air.so2 / countSampleInHour;
    airTemp.no2 = air.no2 / countSampleInHour;
    airTemp.pm2_5 = air.pm2_5 / countSampleInHour;
    airTemp.pm10 = air.pm10 / countSampleInHour;
  }
  else
  {
    airTemp.o3 = 0;
    airTemp.so2 = 0;
    airTemp.co = 0;
    airTemp.no2 = 0;
    airTemp.pm2_5 = 0;
    airTemp.pm10 = 0;
  }
  return airTemp;
};

void receiveEvent(int args)
{
  while (Wire.available())
  {

    int cmd = Wire.read();
    if (cmd == CMD_SEND_DATA_MODE)
    {

      Air temp = GetAirQualityIndexInHour();
      initAirIndexs();
      StaticJsonDocument<1024> doc;
      doc["temperature"] = temp.temperature;
      doc["humidity"] = temp.humidity;
      doc["co"] = temp.co;
      doc["no2"] = temp.no2;
      doc["o3"] = temp.o3;
      doc["so2"] = temp.so2;
      doc["pm2_5"] = temp.pm2_5;
      doc["pm10"] = temp.pm10;

      String output;
      serializeJson(doc, output);
      //  int lenOutput=output.length();
      //  char buff[lenOutput];
      //  output.toCharArray(buff,lenOutput);
      Wire.beginTransmission(I2C_MASTER);
      //  Wire.write(lenOutput);
      //  Wire.write(buff,size_t(buff));
      Wire.println(output);
      Wire.endTransmission();
    }
  }
}

void setup()
{
  delay(1000);
  Serial.begin(9600);
  Wire.begin(I2C_OWN);
  Wire.onReceive(receiveEvent);
  dht.begin();
  // gas.calibrate();
  bool sensorConnected = mics6814.begin(I2C_OWN);

  if (sensorConnected == true) {
    // Print status message
    Serial.println("Connected to MiCS-6814 sensor");

    // Turn heater element on
   mics6814.powerOn();
    
    // Print header for live values
    Serial.println("Current concentrations:");
    Serial.println("CO\tNO2");
  } else {
    // Print error message on failed connection
    Serial.println("Couldn't connect to MiCS-6814 sensor");
  }

  MQ131.begin(MQ131DiGITALPIN, MQ131ANAPIN, LOW_CONCENTRATION, 1000000);
  MQ131.calibrate();
  pms.passiveMode();
}

void loop()
{
  ReadAirQualityIndex();
  delay(60000);
  // put your main code here, to run repeatedly:
}