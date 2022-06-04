//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SensorAnomaly]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorAnomalyDto
    {
        public int SensorAnomalyID { get; set; }
        public SensorDto Sensor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }

    public partial class SensorAnomalySimpleDto
    {
        public int SensorAnomalyID { get; set; }
        public int SensorID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }

}