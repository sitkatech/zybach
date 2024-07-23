using System;

namespace Zybach.Models.Helpers;

public static class RunoffCalculatorHelper
{
    /// <summary>
    /// Calculates the runoff based on the curve number and precipitation. Please see the following link for more information: https://en.wikipedia.org/wiki/Runoff_curve_number.
    /// </summary>
    /// <param name="curveNumber">The curve number (CN) which indicates the runoff potential of the area.</param>
    /// <param name="precipitation">The precipitation amount (P) in inches.</param>
    /// <returns>The calculated runoff (Q) in inches.</returns>
    public static double Runoff(double curveNumber, double precipitation)
    {
        // Calculate the potential maximum soil moisture retention (S)
        var soilMoistureRetention = CalculateSoilMoistureRetention(curveNumber);
            
        // Calculate the initial abstraction (Ia) which is the initial loss before runoff begins
        var initialAbstraction = CalculateInitialAbstraction(soilMoistureRetention);

        // Calculate the runoff (Q) based on the precipitation, initial abstraction, and soil moisture retention
        var runOff = CalculateRunoff(precipitation, initialAbstraction, soilMoistureRetention);
        return runOff;
    }

    /// <summary>
    /// Calculates the potential maximum soil moisture retention (S) based on the curve number (CN).
    /// </summary>
    /// <param name="curveNumber">The curve number (CN).</param>
    /// <returns>The potential maximum soil moisture retention (S) in inches.</returns>
    private static double CalculateSoilMoistureRetention(double curveNumber)
    {
        var soilMoistureRetention = (1000 / curveNumber) - 10;
        return soilMoistureRetention;
    }

    /// <summary>
    /// Calculates the initial abstraction (Ia) which is the amount of water before runoff begins.
    /// </summary>
    /// <param name="soilMoistureRetention">The potential maximum soil moisture retention (S) in inches.</param>
    /// <param name="factor">The factor for calculating initial abstraction, default is 0.2.</param>
    /// <returns>The initial abstraction (Ia) in inches.</returns>
    private static double CalculateInitialAbstraction(double soilMoistureRetention, double factor = .2)
    {
        var initialAbstraction = soilMoistureRetention * factor;
        return initialAbstraction;
    }

    /// <summary>
    /// Calculates the runoff (Q) based on the precipitation (P), initial abstraction (Ia), and soil moisture retention (S).
    /// </summary>
    /// <param name="precipitation">The precipitation amount (P) in inches.</param>
    /// <param name="initialAbstraction">The initial abstraction (Ia) in inches.</param>
    /// <param name="soilMoistureRetention">The potential maximum soil moisture retention (S) in inches.</param>
    /// <returns>The calculated runoff (Q) in inches.</returns>
    private static double CalculateRunoff(double precipitation, double initialAbstraction, double soilMoistureRetention)
    {
        // If precipitation is less than or equal to initial abstraction, there is no runoff.
        if (precipitation <= initialAbstraction)
        {
            return 0;
        }

        var runOff = Math.Pow(precipitation - initialAbstraction, 2) / (precipitation - initialAbstraction + soilMoistureRetention);
        return runOff;
    }
}