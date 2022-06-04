//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[MeasurementType]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class MeasurementTypeDto
    {
        public int MeasurementTypeID { get; set; }
        public string MeasurementTypeName { get; set; }
        public string MeasurementTypeDisplayName { get; set; }
        public string InfluxMeasurementName { get; set; }
        public string InfluxFieldName { get; set; }
    }

    public partial class MeasurementTypeSimpleDto
    {
        public int MeasurementTypeID { get; set; }
        public string MeasurementTypeName { get; set; }
        public string MeasurementTypeDisplayName { get; set; }
        public string InfluxMeasurementName { get; set; }
        public string InfluxFieldName { get; set; }
    }

}