using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AirQualityCalculator
{
    // Determine the breakpoints for each sub-index (values are assumed)
    static readonly int[] breakpoints_PM2_5 = { 0, 10, 20, 25, 50, 75, 800 };
    static readonly int[] breakpoints_PM10 = { 0, 20, 40, 50, 100, 150, 1200 };
    static readonly int[] breakpoints_NO2 = { 0, 40, 90, 120, 230, 340, 1000 };
    static readonly int[] breakpoints_O3 = { 0, 50, 100, 130, 240, 380, 800 };
    static readonly int[] breakpoints_CO2 = { 0, 2000, 4000, 6000, 8000, 10000 };

    public static AirQualityIndex CalculateEU_AQI(double PM2_5, double PM10, double NO2, double O3, double CO2)
    {
        // Map the sub-indices to the AQI scale
        int AQI_PM2_5 = MapToAQIScale(PM2_5, breakpoints_PM2_5);
        int AQI_PM10 = MapToAQIScale(PM10, breakpoints_PM10);
        int AQI_NO2 = MapToAQIScale(NO2, breakpoints_NO2);
        int AQI_O3 = MapToAQIScale(O3, breakpoints_O3);
        int AQI_CO2 = MapToAQIScale(CO2, breakpoints_CO2);

        // Determine the overall AQI by selecting the maximum AQI value
        int overallAQI = Math.Max(AQI_PM2_5, Math.Max(AQI_PM10, Math.Max(AQI_NO2, Math.Max(AQI_O3, AQI_CO2))));

        // Convert to Quality Value (e.g. Unhealthy)
        AirQualityIndex airQuality = GetAirQuality(overallAQI);
        return airQuality;
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

    private static AirQualityIndex GetAirQuality(int AQI)
    {
        if (AQI <= 0)
        {
            return AirQualityIndex.Good;
        }
        else if (AQI <= 1)
        {
            return AirQualityIndex.Fair;
        }
        else if (AQI <= 2)
        {
            return AirQualityIndex.Moderate;
        }
        else if (AQI <= 3)
        {
            return AirQualityIndex.Poor;
        }
        else if (AQI <= 4)
        {
            return AirQualityIndex.Unhealthy;
        }
        else
        {
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

