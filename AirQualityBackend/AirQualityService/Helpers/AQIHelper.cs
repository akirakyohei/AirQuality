using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AirQualityService.Data.Interface;
using AirQualityService.Helpers.@interface;
using AirQualityService.model;
using MongoDB.Driver;


namespace AirQualityService.Helpers
{
    public class AQIHelper : IAQIHelper
    {
        private int[] I = { 0, 50, 100, 150, 200, 300, 400, 500 };
        private int[] O3 = { 0, 160, 200, 300, 400, 800, 1000, 1200 };
        private int[] CO = { 0, 10000, 30000, 45000, 60000, 90000, 120000, 150000 };
        private int[] SO2 = { 0, 125, 350, 550, 800, 1600, 2100, 2630 };
        private int[] NO2 = { 0, 100, 200, 700, 1200, 2350, 3100 };
        private int[] PM10 = { 0, 50, 150, 250, 350, 420, 500, 600 };
        private int[] PM2_5 = { 0, 25, 50, 80, 150, 250, 350, 500 };

        private readonly IAirQualityRepository airQualityRepository;

        public AQIHelper(IAirQualityRepository airQualityRepository)
        {
            this.airQualityRepository = airQualityRepository;
        }



        //get hour
        public int GetAQIInHour(AirQuality air)
        {

            int aqi = GetAQIwithO3(air.O3);
            aqi = Math.Max(aqi, GetAQIwithSO2(air.SO2));
            aqi = Math.Max(aqi, GetAQIwithCO(air.CO));
            aqi = Math.Max(aqi, GetAQIwithNO2(air.NO2));


            var arrAir = airQualityRepository.GetAirQualityNowLimit(air.PointId, 11);
            arrAir.Add(air);

            foreach (var item in arrAir)
            {
                Console.Write("\t" + item.PM2_5);
            }
            Console.WriteLine();



            List<float> arrPm2_5 = new List<float>(12);
            List<float> arrPm10_0 = new List<float>(12);
            for (int i = 0; i < 12; i++)
            {
                arrPm2_5.Add(0);
                arrPm10_0.Add(0);
            }

            var currentTime = air.DateTime;

            foreach (var item in arrAir)
            {
                var time = item.DateTime;
                TimeSpan duration = currentTime - time;
                if (duration.TotalHours > 11) continue;
                Console.WriteLine("duration " + duration.TotalHours);
                arrPm2_5[(int)duration.TotalHours] = item.PM2_5;
                arrPm10_0[(int)duration.TotalHours] = item.PM10_0;

            }

            foreach (var item in arrPm2_5)
            {
                Console.Write("\t" + item);
            }
            Console.WriteLine();
            var nowcastPm2_5 = GetNowcast(arrPm2_5);

            if (nowcastPm2_5 != -1)
            {
                aqi = Math.Max(aqi, GetAQIwithPM2_5(nowcastPm2_5));

            }

            var nowcastPm10_0 = GetNowcast(arrPm10_0);

            if (nowcastPm10_0 != -1)
            {
                aqi = Math.Max(aqi, GetAQIwithPM2_5(nowcastPm10_0));

            }

            return aqi;
        }

        //get day
        public int GetAQIInDay(Guid idPoint)
        {
            Console.WriteLine(idPoint);
            var arrAirQuality = airQualityRepository.GetAirQualityNowLimit(idPoint, 32);

            if (arrAirQuality.Count() == 0) return 0;

            DateTime now = DateTime.Now;
            DateTime last = arrAirQuality[arrAirQuality.Count() - 1].DateTime;
            if (now.Year == last.Year && now.Month == last.Month && now.Day == last.Day) return 0;
            float pm2_5 = 0, pm10_0 = 0, so2 = 0, no2 = 0, co = 0, o3 = 0;

            List<float> arrO3 = new List<float>(32);
            List<float> arrCO = new List<float>(24);
            List<float> arrNO2 = new List<float>(24);
            List<float> arrSO2 = new List<float>(24);
            List<float> arrPm2_5 = new List<float>(24);
            List<float> arrPm10_0 = new List<float>(24);

            for (int i = 0; i < 32; i++)
            {
                arrO3.Add(0);
            }
            for (int i = 0; i < 24; i++)
            {
                arrCO.Add(0);
                arrNO2.Add(0);
                arrSO2.Add(0);
                arrPm2_5.Add(0);
                arrPm10_0.Add(0);
            }

            Console.WriteLine(arrAirQuality.Count());
            var currentTime = arrAirQuality[arrAirQuality.Count() - 1].DateTime;

            foreach (var item in arrAirQuality)
            {
                var time = item.DateTime;
                TimeSpan duration = currentTime - time;
                Console.WriteLine("duration " + duration.TotalHours);
                if (duration.TotalHours > 31) continue;
                arrO3[(int)duration.TotalHours] = item.O3;

                if (duration.TotalHours > 23) continue;
                arrSO2[(int)duration.TotalHours] = item.SO2;
                arrCO[(int)duration.TotalHours] = item.CO;
                arrNO2[(int)duration.TotalHours] = item.NO2;
                arrPm2_5[(int)duration.TotalHours] = item.PM2_5;
                arrPm10_0[(int)duration.TotalHours] = item.PM10_0;
            }


            float tb8h = 0;
            float pm2_5Sum = 0;
            float pm10_0Sum = 0;
            for (int i = 0; i < 24; i++)
            {
                //o3
                o3 = Math.Max(o3, arrO3[i]);
                if (i == 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tb8h += arrO3[j];
                    }

                    o3 = Math.Max(o3, tb8h / 8);
                }
                else
                {
                    tb8h -= arrO3[i - 1];
                    tb8h += arrO3[i + 8];
                    o3 = Math.Max(o3, tb8h / 8);
                }

                //so2
                so2 = Math.Max(so2, arrSO2[i]);
                //co
                co = Math.Max(co, arrCO[i]);
                //no2
                no2 = Math.Max(no2, arrNO2[i]);
                //pm2_5
                pm2_5Sum += arrPm2_5[i];
                pm10_0Sum += arrPm10_0[i];
            }
            pm10_0 = pm10_0Sum / 24;
            pm2_5 = pm2_5Sum / 24;

            //aqi
            var aqi = GetAQIwithO3(o3);
            aqi = Math.Max(aqi, GetAQIwithCO(co));
            aqi = Math.Max(aqi, GetAQIwithNO2(no2));
            aqi = Math.Max(aqi, GetAQIwithSO2(so2));
            aqi = Math.Max(aqi, GetAQIwithPM2_5(pm2_5));
            aqi = Math.Max(aqi, GetAQIwithPM10_0(pm10_0));

            return aqi;
        }

        //nowcast
        private float GetNowcast(List<float> arr)
        {

            if (arr.Count() < 12)
            {
                throw new Exception("Array must length by 12 .");
            }

            // du lieu phai cos it nhat 2 trong 3 c1,c2,c3
            int minCi = 0;
            if (arr[11] != 0) minCi++;
            if (arr[10] != 0) minCi++;
            if (arr[9] != 0) minCi++;
            if (minCi < 2) return -1;


            //tinh w
            var min = arr[0];
            var max = arr[0];

            for (int i = 0; i < 12; i++)
            {
                var item = arr[i];
                min = item < min ? item : min;
                max = item > max ? item : max;
            }


            var w = min / max;
            Console.WriteLine("w " + w);


            if (w < 0.5)
            {
                var nowcast = 0d;
                var s = 1d;

                for (int i = 0; i < 12; i++)
                {
                    s = s * 0.5;
                    nowcast += arr[i] * s;
                }

                return (float)(Math.Round(nowcast, 1));
            }
            else
            {
                var nowcast = 0d;
                var tnow = 0d;
                var s = 1d;
                for (int i = 0; i < 12; i++)
                {
                    if (arr[i] != 0)
                    {
                        nowcast += s * arr[i];
                        tnow += s;
                    }
                    s *= w;
                }

                return (float)(Math.Round(nowcast / tnow, 1));
            }


        }

        //o3
        private int GetAQIwithO3(float o3)
        {
            int underLimito3 = 0, upLimito3 = 0;
            if (o3 <= 0)
            {
                throw new Exception("O3 must greater than 0.");
            }

            if (o3 >= O3[O3.Count() - 1])
            {
                underLimito3 = 6;
                upLimito3 = 7;

            }
            for (int i = 0; i < O3.Count(); i++)
            {
                if (o3 <= O3[i])
                {
                    upLimito3 = i;
                    underLimito3 = i - 1;
                    break;
                }
            }

            int AQIx = (int)Math.Round((I[upLimito3] - I[underLimito3]) * 1.0 / (O3[upLimito3] - O3[underLimito3]) * (o3 - O3[underLimito3]) + I[underLimito3]);
            return AQIx;
        }

        //no3
        private int GetAQIwithNO2(float no2)
        {
            int underLimitno2 = 0, upLimitno2 = 0;
            if (no2 <= 0)
            {
                throw new Exception("no2 must greater than 0.");
            }
            if (no2 >= NO2[NO2.Count() - 1])
            {
                underLimitno2 = 6;
                upLimitno2 = 7;

            }
            for (int i = 0; i < NO2.Count(); i++)
            {
                if (no2 <= NO2[i])
                {
                    upLimitno2 = i;
                    underLimitno2 = i - 1;
                    break;
                }
            }

            int AQIx = (int)Math.Round(((I[upLimitno2] - I[underLimitno2]) * 1.0 / (NO2[upLimitno2] - NO2[underLimitno2]) * (no2 - NO2[underLimitno2]) + I[underLimitno2]));

            return AQIx;
        }

        //co
        private int GetAQIwithCO(float co)
        {
            int underLimitco = 0, upLimitco = 0;
            if (co <= 0)
            {
                throw new Exception("O3 must greater than 0.");
            }
            if (co >= CO[CO.Count() - 1])
            {
                underLimitco = 6;
                upLimitco = 7;


            }
            for (int i = 0; i < CO.Count(); i++)
            {
                if (co <= CO[i])
                {
                    upLimitco = i;
                    underLimitco = i - 1;
                    break;
                }
            }

            int AQIx = (int)Math.Round(((I[upLimitco] - I[underLimitco]) * 1.0 / (CO[upLimitco] - CO[underLimitco]) * (co - CO[underLimitco]) + I[underLimitco]));

            return AQIx;
        }

        //so2
        private int GetAQIwithSO2(float so2)
        {
            int underLimitso2 = 0, upLimitso2 = 0;
            if (so2 <= 0)
            {
                throw new Exception("SO2 must greater than 0.");
            }
            if (so2 >= SO2[SO2.Count() - 1])
            {
                underLimitso2 = 6;
                upLimitso2 = 7;

            }
            for (int i = 0; i < SO2.Count(); i++)
            {
                if (so2 <= SO2[i])
                {
                    upLimitso2 = i;
                    underLimitso2 = i - 1;
                    break;
                }
            }

            int AQIx = (int)Math.Round(((I[upLimitso2] - I[underLimitso2]) * 1.0 / (SO2[upLimitso2] - SO2[underLimitso2]) * (so2 - SO2[underLimitso2]) + I[underLimitso2]));

            return AQIx;
        }

        //pm2_5
        private int GetAQIwithPM2_5(float pm2_5)
        {
            int underLimitpm2_5 = 0, upLimitpm2_5 = 0;
            if (pm2_5 <= 0)
            {
                throw new Exception("pm2_5 must greater than 0.");
            }

            if (pm2_5 >= PM2_5[PM2_5.Count() - 1])
            {
                underLimitpm2_5 = 6;
                upLimitpm2_5 = 7;

            }
            for (int i = 0; i < PM2_5.Count(); i++)
            {

                if (pm2_5 <= PM2_5[i])
                {
                    upLimitpm2_5 = i;
                    underLimitpm2_5 = i - 1;
                    break;
                }
            }
            Console.WriteLine(pm2_5);
            Console.WriteLine(upLimitpm2_5 + " " + underLimitpm2_5);
            Console.WriteLine($"I {I[underLimitpm2_5]}  {I[upLimitpm2_5]}");
            Console.WriteLine($"BRP {PM2_5[underLimitpm2_5]}  {PM2_5[upLimitpm2_5]}");

            int AQIx = (int)Math.Round((I[upLimitpm2_5] - I[underLimitpm2_5]) * 1.0 / (PM2_5[upLimitpm2_5] - PM2_5[underLimitpm2_5]) * (pm2_5 - PM2_5[underLimitpm2_5]) + I[underLimitpm2_5]);

            return AQIx;
        }

        //pm10_0
        private int GetAQIwithPM10_0(float pm10_0)
        {
            int underLimitpm10_0 = 0, upLimitpm10_0 = 0;
            if (pm10_0 <= 0)
            {
                throw new Exception("pm10_0 must greater than 0.");
            }
            if (pm10_0 >= PM10[PM10.Count() - 1])
            {
                underLimitpm10_0 = 6;
                upLimitpm10_0 = 7;

            }
            for (int i = 0; i < PM10.Count(); i++)
            {
                if (pm10_0 <= PM10[i])
                {
                    upLimitpm10_0 = i;
                    underLimitpm10_0 = i - 1;
                    break;
                }
            }

            int AQIx = (int)Math.Round(((I[upLimitpm10_0] - I[underLimitpm10_0]) * 1.0 / (PM10[upLimitpm10_0] - PM10[underLimitpm10_0]) * (pm10_0 - PM10[underLimitpm10_0]) + I[underLimitpm10_0]));

            return AQIx;
        }


    }
}
