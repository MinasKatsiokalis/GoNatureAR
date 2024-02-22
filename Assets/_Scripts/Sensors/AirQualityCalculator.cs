using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GoNatureAR.Sensors
{
    public static class AirQualityCalculator
    {
        // Determine the breakpoints for each sub-index (values are assumed)
        static readonly int[] breakpoints_PM10 = { 0, 20, 40, 50, 100, 150, 1200 };
        static readonly int[] breakpoints_PM2_5 = { 0, 10, 20, 25, 50, 75, 800 };
        static readonly int[] breakpoints_NO2 = { 0, 40, 90, 120, 230, 340, 1000 };
        static readonly int[] breakpoints_O3 = { 0, 50, 100, 130, 240, 380, 800 };
        static readonly int[] breakpoints_SO2 = { 0, 100, 200, 350, 500, 750, 1250 };

        public static int CalculateEU_AQI(double PM2_5, double PM10, double NO2, double O3, double SO2)
        {
            // Map the sub-indices to the AQI scale
            int AQI_PM2_5 = MapToAQIScale(PM2_5, breakpoints_PM2_5);
            int AQI_PM10 = MapToAQIScale(PM10, breakpoints_PM10);
            int AQI_NO2 = MapToAQIScale(NO2, breakpoints_NO2);
            int AQI_O3 = MapToAQIScale(O3, breakpoints_O3);
            int AQI_SO2 = MapToAQIScale(SO2, breakpoints_SO2);

            // Determine the overall AQI by selecting the maximum AQI value
            int overallAQI = Math.Max(AQI_PM2_5, Math.Max(AQI_PM10, Math.Max(AQI_NO2, Math.Max(AQI_O3, AQI_SO2))));

            return overallAQI;
        }

        // Helper function to map concentration to AQI scale
        private static int MapToAQIScale(double concentration, int[] breakpoints)
        {
            for (int i = 0; i < breakpoints.Length - 1; i++)
            {
                if (concentration >= breakpoints[i] && concentration <= breakpoints[i + 1])
                    return i;
            }

            // If the subIndex exceeds the highest breakpoint, return the highest AQI value
            return breakpoints.Length - 1;
        }

        public static AirQualityIndex GetAirQualityIndex(int AQI)
        {
            switch (AQI)
            {
                case int n when n <= 0:
                    return AirQualityIndex.Good;
                case int n when n <= 1:
                    return AirQualityIndex.Fair;
                case int n when n <= 2:
                    return AirQualityIndex.Moderate;
                case int n when n <= 3:
                    return AirQualityIndex.Poor;
                case int n when n <= 4:
                    return AirQualityIndex.Unhealthy;
                default:
                    return AirQualityIndex.Hazardous;
            }
        }
    }

    public enum AirQualityIndex
    {
        Good,
        Fair,
        Moderate,
        Poor,
        Unhealthy,
        Hazardous
    }
}
