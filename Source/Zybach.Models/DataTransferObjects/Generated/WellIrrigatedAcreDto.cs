//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellIrrigatedAcre]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellIrrigatedAcreDto
    {
        public int WellIrrigatedAcreID { get; set; }
        public WellDto Well { get; set; }
        public int IrrigationYear { get; set; }
        public double Acres { get; set; }
    }
}