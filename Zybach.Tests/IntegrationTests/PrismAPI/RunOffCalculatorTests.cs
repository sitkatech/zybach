using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Zybach.Models.Helpers;

namespace Zybach.Tests.IntegrationTests.PrismAPI;

[TestClass]
public class RunOffCalculatorTests
{
    private static MethodInfo _calculateSoilMoistureRetentionMethod;
    private static MethodInfo _calculateInitialAbstractionMethod;
    private static MethodInfo _calculateRunoffMethod;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _calculateSoilMoistureRetentionMethod = typeof(RunoffCalculatorHelper).GetMethod("CalculateSoilMoistureRetention", BindingFlags.NonPublic | BindingFlags.Static);
        _calculateInitialAbstractionMethod = typeof(RunoffCalculatorHelper).GetMethod("CalculateInitialAbstraction", BindingFlags.NonPublic | BindingFlags.Static);
        _calculateRunoffMethod = typeof(RunoffCalculatorHelper).GetMethod("CalculateRunoff", BindingFlags.NonPublic | BindingFlags.Static);
    }


    [DataTestMethod]
    [DataRow(75, 3.33)]
    [DataRow(100, 0)]
    [DataRow(50, 10)]
    public async Task CalculateSoilMoistureRetention_ReturnsCorrectSoilMoistureRetention(double CN, double expected)
    {
        var soilMoistureRetention = (double) _calculateSoilMoistureRetentionMethod.Invoke(null, new object[] { CN })!;
        var result = Math.Round(soilMoistureRetention, 2);
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(10, 0.2, 2)]
    [DataRow(20, 0.05, 1)]
    [DataRow(30, 0.2, 6)]
    public void CalculateInitialAbstraction_ReturnsCorrectInitialAbstraction(double S, double factor, double expected)
    {
        var initialAbstraction = (double) _calculateInitialAbstractionMethod.Invoke(null, new object[] { S, factor })!;
        var result = Math.Round(initialAbstraction, 2);
        Assert.AreEqual(expected, result);
    } 

    [DataTestMethod] 
    [DataRow(0, 0.2, 1, 0)]
    public void CalculateRunoff_ReturnsCorrectRunoff(double precipitation, double initialAbstraction, double soilMoistureMaximum, double expected)
    {
        var runOff = (double)_calculateRunoffMethod.Invoke(null, new object[] { precipitation, initialAbstraction, soilMoistureMaximum })!;
        var result = Math.Round(runOff, 2);
        Assert.AreEqual(expected, result);
    }

    [DataTestMethod]
    [DataRow(61, 5, 1.37)]
    public void Runoff_ReturnsCorrectRunoff(double cn, double precipitation, double expected)
    {
        var runOff = RunoffCalculatorHelper.Runoff(cn, precipitation);
        var result = Math.Round(runOff, 2);
        Assert.AreEqual(expected, result);
    }
}