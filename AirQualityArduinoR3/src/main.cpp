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
#define CMD_SEND_DATA_MODE '0'

// address master
#define I2C_MASTER 8
#define I2C_OWN 6

// ------------------------------------
#define DHTPIN 2       //temp , humidity
#define DHTTYPE DHT22  //
#define MQ131ANAPIN A0 //o3
#define MQ131DiGITALPIN 5
#define MQ136PIN A1     //so2
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
int countSampleInHour;
Air air;
void ReadAirQualityIndex();

float randomDataFloat(float value, int denta)
{
  if (denta > 100)
    denta = 100;
  if (denta < 0)
    denta = 0;

  randomSeed(millis());
  int percent = random(0, denta * 10 + 1);
  int operation = random(0, 2);
  if (operation == 1)
  {
    value = value + percent * value / 1000;
  }
  else
  {
    value = value - percent * value / 1000;
    if (value < 0)
      return 0;
  }
  return value;
}

int randomDataInt(int value, int denta)
{
  if (denta > 100)
    denta = 100;
  if (denta < 0)
    denta = 0;

  randomSeed(millis());
  int percent = random(0, denta);
  randomSeed(millis());
  int operation = random(0, 2);
  if (operation == 1)
  {
    value = value + percent;
  }
  else
  {
    value = value - percent;
    if (value < 0)
      return 0;
  }
  return value;
}
void initAirIndexs()
{
  air.temperature = 45;
  air.humidity = 60;
  air.o3 = 46;
  air.so2 = 45;
  air.co = 3565;
  air.no2 = 5634;
  air.pm2_5 = 45;
  air.pm10 = 235;
  countSampleInHour = 0;
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

void ReadAirQualityIndex()
{
  air.temperature = randomDataFloat(air.temperature, 3);
  air.o3 = randomDataFloat(air.o3, 3);
  air.humidity = randomDataFloat(air.humidity, 4);
  air.co = randomDataFloat(air.co, 4);
  air.no2 = randomDataFloat(air.no2, 4);
  air.so2 = randomDataFloat(air.so2, 4);
  air.pm2_5 = randomDataFloat(air.pm2_5, 4);
  air.pm10 = randomDataFloat(air.pm10, 45);
  countSampleInHour++;
}

// void receiveEvent(int howMany);
void receiveEvent(int howMany);
void requestEvent();
void setup()
{
  Wire.begin(8);                /* join i2c bus with address 8 */
  Wire.onReceive(receiveEvent); /* register receive event */
  Wire.onRequest(requestEvent); /* register request event */
  Serial.begin(9600);           /* start serial for debug */
}

void loop()
{
  ReadAirQualityIndex();
  delay(600);
}

// function that executes whenever data is received from master
void receiveEvent(int howMany)
{
  while (0 < Wire.available())
  {
    char c = Wire.read(); /* receive byte as a character */
    Serial.print(c);      /* print the character */
  }
  Serial.println(); /* to newline */
}

// function that executes whenever data is requested from master
void requestEvent()
{

  Air airTemp = GetAirQualityIndexInHour();

  float dataArray[8] = {
      airTemp.temperature,
      airTemp.humidity,
      airTemp.o3,
      airTemp.co,
      airTemp.no2,
      airTemp.so2,
      airTemp.pm2_5,
      airTemp.pm10};

  uint8_t *pData = (uint8_t *)dataArray;

  for (int i = 0; i < 8 * sizeof(float); i++)
  {

    Wire.write(*pData++); //data bytes are queued in local buffer
  }

  initAirIndexs();
}

/*
Air air;
//MICS6814 gas(COPIN, NO2PIN, 0);
MiCS6814 mics6814;
//  Thực tế cần thay đổi thời gian hiệu chỉnh 25 phút
MQ_136 mqt136(MQ136PIN);
DHT dht(DHTPIN, DHTTYPE);
PMS pms(Serial);
PMS::DATA data;
int countSampleInHour;
bool get_sample = false;

void ReadAirQualityIndex();

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

// https://how2electronics.com/iot-air-pollution-monitoring-esp8266/?fbclid=IwAR3mk5WB5F8ARIpocQDmNa7EVDJPxa3co2GIhWfpU0ibgRBg59YnGgfvGgE

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
  float co = mics6814.measureCO();
  Serial.print("CO :");
  Serial.println(co);
  if (co <= 0)
    co = 0;
  air.co += co;
}
void readNO2()
{
  // air.co = gas.measure(NO2);
  float no2 = mics6814.measureNO2();
  Serial.print("NO2 :");
  Serial.println(no2);
  if (no2 <= 0)
    no2 = 0;
  air.no2 += no2;
}
void readO3()
{
  MQ131.setEnv(air.temperature, air.humidity);
  float o3 = MQ131.getO3(UG_M3);
  Serial.print("O3 :");
  Serial.println(o3);
  if (o3 <= 0)
    o3 = 0;
  air.o3 += o3;
}
void readSO2()
{
  float so2 = mqt136.getSo2();
  Serial.print("SO2 :");
  Serial.println(so2);
  if (so2 <= 0)
    so2 = 0;
  air.so2 += so2;
}
void readPM()
{
  pms.wakeUp();
  delay(30000);
  pms.requestRead();
  if (pms.readUntil(data))
  {
    float pm2_5 = data.PM_AE_UG_2_5;
    Serial.println();
    Serial.print("\npm2_5 :");
    Serial.println(pm2_5);
    if (pm2_5 <= 0)
      pm2_5 = 0;
    air.pm2_5 += pm2_5;

    float pm10 = data.PM_AE_UG_10_0;
    Serial.println("\n");
    Serial.print("\npm10 :");
    Serial.println(pm10);
    Serial.println("\n");
    if (pm10 <= 0)
      pm10 = 0;
    air.pm10 += pm10;
  }
  else
  {
    Serial.println("\nNot read data pm");
  }
  pms.sleep();
}

void ReadAirQualityIndex()
{
  Serial.println("open");
  readEnv();
  readCO();
  readNO2();
  readO3();
  readSO2();
  // readPM();
  countSampleInHour++;
  Serial.println("close");
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

  Serial.println("receive I2c");
  delay(100);
  if (0 < Wire.available())
  {

    char cmd = Wire.read();
    // Serial.println(cmd);
    // Serial.println(cmd);
    if (cmd == CMD_SEND_DATA_MODE)
    {
      get_sample = true;
    }
  }
}

void setup()
{
  delay(1000);
  Serial.begin(9600);

  //delay(200);
  Serial.println("begin");
  Wire.begin(I2C_OWN);
  Wire.onReceive(receiveEvent);
  dht.begin();
  // gas.calibrate();
  bool sensorConnected = mics6814.begin();

  if (sensorConnected == true)
  {
    // Print status message
    Serial.println("Connected to MiCS-6814 sensor");

    // Turn heater element on
    mics6814.powerOn();

    // Print header for live values
    Serial.println("Current concentrations:");
    Serial.println("CO\tNO2");
  }
  else
  {
    // Print error message on failed connection
    Serial.println("Couldn't connect to MiCS-6814 sensor");
  }
  // MQ131.begin(MQ131DiGITALPIN, MQ131ANAPIN, LOW_CONCENTRATION, 1000000);
  // MQ131.calibrate();
  pms.passiveMode();
  Serial.print("23");
}
void loop()
{
  Serial.println("\nloop");

  if (get_sample)
  {
    get_sample = false;
    Serial.println("cmd");
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
    Serial.println(output);
    Wire.println(output);
    Wire.endTransmission();
  }

  ReadAirQualityIndex();
  delay(1000);

  // put your main code here, to run repeatedly:
}
*/