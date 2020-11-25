
#define CALIBRATION_SAMPLE_TIMES (50)     //define how many samples you are going to take in the calibration phase
#define CALIBRATION_SAMPLE_INTERVAL (500) //define the time interal(in milisecond) between each samples in the cablibration phase
#define READ_SAMPLE_INTERVAL (50)         //define how many samples you are going to take in normal operation
#define READ_SAMPLE_TIMES (5)             //define the time interal(in milisecond) between each samples in

#define CO2ppm 500
#define COppm 10
#define NH4ppm 10
#define C2H50Hppm 10
#define CH3ppm 10
#define CH3_2COppm 10
#define SO2ppm 10
#define CH4ppm 10
#define COppm 10
#define H2Sppm 10
#define NH4ppm 10


class MQ_136
{
private:

float SO2_136Curve[2] = {40.44109566, -1.085728557}; //MQ136 http://china-total.com/product/meter/gas-sensor/MQ136.pdf
float CH4_136Curve[2] = {57.82777729, -1.187494933}; //MQ136 http://china-total.com/product/meter/gas-sensor/MQ136.pdf
float CO_136Curve[2] = {2142.297846, -2.751369226};  //MQ136 http://china-total.com/product/meter/gas-sensor/MQ136.pdf
float H2S_136Curve[2] = {0, 0};                      //MQ136 http://www.sensorica.ru/pdf/MQ-136.pdf
float NH4_136Curve[2] = {0, 0};                      //MQ136 http://www.sensorica.ru/pdf/MQ-136.pdf



    int pin_136;
    float Ro136;      //MQ136 ... this has to be tuned 10K Ohm
    float RL = 0.990; //FC-22

    /****************** MQResistanceCalculation ****************************************
Input:   raw_adc - raw value read from adc, which represents the voltage
Output:  the calculated sensor resistance
Remarks: The sensor and the load resistor forms a voltage divider. Given the voltage
         across the load resistor and its resistance, the resistance of the sensor
         could be derived.
************************************************************************************/
    float MQResistanceCalculation(int raw_adc, float rl_value);

    /***************************** MQCalibration ****************************************
Input:   mq_pin - analog channel
Output:  Ro of the sensor
Remarks: This function assumes that the sensor is in clean air. It use
         MQResistanceCalculation to calculates the sensor resistance in clean air.        .
************************************************************************************/
    float MQCalibration(int mq_pin, double ppm, double rl_value, float *pcurve);

    /*****************************  MQRead *********************************************
Input:   mq_pin - analog channel
Output:  Rs of the sensor
Remarks: This function use MQResistanceCalculation to caculate the sensor resistenc (Rs).
         The Rs changes as the sensor is in the different consentration of the target
         gas. The sample times and the time interval between samples could be configured
         by changing the definition of the macros.
************************************************************************************/
    float MQRead(int mq_pin, float rl_value);

    /*****************************  MQGetPercentage **********************************
Input:   rs_ro_ratio - Rs divided by Ro
         pcurve      - pointer to the curve of the target gas
Output:  ppm of the target gas
Remarks: By using the slope and a point of the line. The x(logarithmic value of ppm)
         of the line could be derived if y(rs_ro_ratio) is provided. As it is a
         logarithmic coordinate, power of 10 is used to convert the result to non-logarithmic
         value.
************************************************************************************/
    float MQGetPercentage(float rs_ro_ratio, float ro, float *pcurve);

public:
    MQ_136(int pin);

    float getSo2();
};