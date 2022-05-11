//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnit]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitDto
    {
        public int AgHubIrrigationUnitID { get; set; }
        public string WellTPID { get; set; }
        public double? IrrigationUnitAreaInAcres { get; set; }
    }

    public partial class AgHubIrrigationUnitSimpleDto
    {
        public int AgHubIrrigationUnitID { get; set; }
        public string WellTPID { get; set; }
        public double? IrrigationUnitAreaInAcres { get; set; }
    }

}