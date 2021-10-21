//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Sensor]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorDto
    {
        public int SensorID { get; set; }
        public string SensorName { get; set; }
        public SensorTypeDto SensorType { get; set; }
        public WellDto Well { get; set; }
        public bool InGeoOptix { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}