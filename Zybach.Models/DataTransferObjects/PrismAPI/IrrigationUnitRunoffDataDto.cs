using System;

namespace Zybach.Models.DataTransferObjects;

public class IrrigationUnitRunoffDataDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }

    public double Precipitation { get; set; }
    public double CurveNumber { get; set; }
    public double RunoffDepth { get; set; }
    public double RunoffVolume { get; set; }
}