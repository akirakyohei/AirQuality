#include <MQ_136.h>
#include <Arduino.h>

/****************** MQResistanceCalculation ****************************************
Input:   raw_adc - raw value read from adc, which represents the voltage
Output:  the calculated sensor resistance
Remarks: The sensor and the load resistor forms a voltage divider. Given the voltage
         across the load resistor and its resistance, the resistance of the sensor
         could be derived.
************************************************************************************/
float MQ_136::MQResistanceCalculation(int raw_adc, float rl_value)
{
    //  return ( ((float)rl_value*(1023-raw_adc)/raw_adc));
    return (long)((long)(1024 * 1000 * (long)rl_value) / raw_adc - (long)rl_value);
    ;
}

/***************************** MQCalibration ****************************************
Input:   mq_pin - analog channel
Output:  Ro of the sensor
Remarks: This function assumes that the sensor is in clean air. It use
         MQResistanceCalculation to calculates the sensor resistance in clean air.        .
************************************************************************************/
float MQ_136::MQCalibration(int mq_pin, double ppm, double rl_value, float *pcurve)
{
    int i;
    float val = 0;

    for (i = 0; i < CALIBRATION_SAMPLE_TIMES; i++) //take multiple samples
    {
        val += MQResistanceCalculation(analogRead(mq_pin), rl_value);
        delay(CALIBRATION_SAMPLE_INTERVAL);
    }
    val = val / CALIBRATION_SAMPLE_TIMES; //calculate the average value
    //Ro = Rs * sqrt(a/ppm, b) = Rs * exp( ln(a/ppm) / b )

    return (long)val * exp((log(pcurve[0] / ppm) / pcurve[1]));
}
/*****************************  MQRead *********************************************
Input:   mq_pin - analog channel
Output:  Rs of the sensor
Remarks: This function use MQResistanceCalculation to caculate the sensor resistenc (Rs).
         The Rs changes as the sensor is in the different consentration of the target
         gas. The sample times and the time interval between samples could be configured
         by changing the definition of the macros.
************************************************************************************/
float MQ_136::MQRead(int mq_pin, float rl_value)
{
    int i;
    float rs = 0;

    for (i = 0; i < READ_SAMPLE_TIMES; i++)
    {
        rs += MQResistanceCalculation(analogRead(mq_pin), rl_value);
        delay(READ_SAMPLE_INTERVAL);
    }

    rs = rs / READ_SAMPLE_TIMES;

    return rs;
}

/*****************************  MQGetPercentage **********************************
Input:   rs_ro_ratio - Rs divided by Ro
         pcurve      - pointer to the curve of the target gas
Output:  ppm of the target gas
Remarks: By using the slope and a point of the line. The x(logarithmic value of ppm)
         of the line could be derived if y(rs_ro_ratio) is provided. As it is a
         logarithmic coordinate, power of 10 is used to convert the result to non-logarithmic
         value.
************************************************************************************/
float MQ_136::MQGetPercentage(float rs_ro_ratio, float ro, float *pcurve)
{
    return (double)(pcurve[0] * pow(((double)rs_ro_ratio / ro), pcurve[1]));
}

MQ_136::MQ_136(int pin)
{
    pin_136 = pin;
    Ro136 = MQCalibration(pin_136, SO2ppm, RL, SO2_136Curve);
    pinMode(pin_136, OUTPUT);
}
float MQ_136::getSo2()
{
    return MQGetPercentage(MQRead(pin_136, RL), Ro136, SO2_136Curve);
}
