//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitRunoff]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitRunoffDto
    {
        public int AgHubIrrigationUnitRunoffID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string CropType { get; set; }
        public string Tillage { get; set; }
        public double CurveNumber { get; set; }
        public double Precipitation { get; set; }
        public double Area { get; set; }
        public double RunoffDepth { get; set; }
        public double RunoffVolume { get; set; }
    }

    public partial class AgHubIrrigationUnitRunoffSimpleDto
    {
        public int AgHubIrrigationUnitRunoffID { get; set; }
        public System.Int32 AgHubIrrigationUnitID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string CropType { get; set; }
        public string Tillage { get; set; }
        public double CurveNumber { get; set; }
        public double Precipitation { get; set; }
        public double Area { get; set; }
        public double RunoffDepth { get; set; }
        public double RunoffVolume { get; set; }
    }

}