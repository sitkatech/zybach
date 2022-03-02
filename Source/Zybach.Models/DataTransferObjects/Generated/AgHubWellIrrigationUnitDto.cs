//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigationUnit]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubWellIrrigationUnitDto
    {
        public int AgHubWellIrrigationUnitID { get; set; }
        public AgHubWellDto AgHubWell { get; set; }
    }

    public partial class AgHubWellIrrigationUnitSimpleDto
    {
        public int AgHubWellIrrigationUnitID { get; set; }
        public int AgHubWellID { get; set; }
    }

}