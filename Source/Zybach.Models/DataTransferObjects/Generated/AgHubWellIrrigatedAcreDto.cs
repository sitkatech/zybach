//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcre]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubWellIrrigatedAcreDto
    {
        public int AgHubWellIrrigatedAcreID { get; set; }
        public AgHubWellDto AgHubWell { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
    }
}