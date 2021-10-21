//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public StreamFlowZoneDto StreamflowZone { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}