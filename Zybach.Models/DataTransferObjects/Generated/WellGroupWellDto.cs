//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellGroupWell]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellGroupWellDto
    {
        public int WellGroupWellID { get; set; }
        public WellGroupDto WellGroup { get; set; }
        public WellDto Well { get; set; }
        public bool IsPrimary { get; set; }
    }

    public partial class WellGroupWellSimpleDto
    {
        public int WellGroupWellID { get; set; }
        public System.Int32 WellGroupID { get; set; }
        public System.Int32 WellID { get; set; }
        public bool IsPrimary { get; set; }
    }

}