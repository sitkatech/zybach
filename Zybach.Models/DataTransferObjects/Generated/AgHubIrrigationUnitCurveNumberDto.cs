//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitCurveNumber]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitCurveNumberDto
    {
        public int AgHubIrrigationUnitCurveNumberID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public string HydrologicSoilGroup { get; set; }
        public double MTillCurveNumber { get; set; }
        public double STillCurveNumber { get; set; }
        public double NTillCurveNumber { get; set; }
        public double CTillCurveNumber { get; set; }
        public double UndefinedTillCurveNumber { get; set; }
    }

    public partial class AgHubIrrigationUnitCurveNumberSimpleDto
    {
        public int AgHubIrrigationUnitCurveNumberID { get; set; }
        public System.Int32 AgHubIrrigationUnitID { get; set; }
        public string HydrologicSoilGroup { get; set; }
        public double MTillCurveNumber { get; set; }
        public double STillCurveNumber { get; set; }
        public double NTillCurveNumber { get; set; }
        public double CTillCurveNumber { get; set; }
        public double UndefinedTillCurveNumber { get; set; }
    }

}