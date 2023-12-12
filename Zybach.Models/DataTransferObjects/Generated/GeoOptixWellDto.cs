//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[GeoOptixWell]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class GeoOptixWellDto
    {
        public int GeoOptixWellID { get; set; }
        public WellDto Well { get; set; }
    }

    public partial class GeoOptixWellSimpleDto
    {
        public int GeoOptixWellID { get; set; }
        public System.Int32 WellID { get; set; }
    }

}