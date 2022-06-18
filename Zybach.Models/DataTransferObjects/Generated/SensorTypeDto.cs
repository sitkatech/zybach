//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SensorType]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorTypeDto
    {
        public int SensorTypeID { get; set; }
        public string SensorTypeName { get; set; }
        public string SensorTypeDisplayName { get; set; }
        public string ChartColor { get; set; }
        public string AnomalousChartColor { get; set; }
        public string YAxisTitle { get; set; }
        public bool ReverseYAxisScale { get; set; }
    }

    public partial class SensorTypeSimpleDto
    {
        public int SensorTypeID { get; set; }
        public string SensorTypeName { get; set; }
        public string SensorTypeDisplayName { get; set; }
        public string ChartColor { get; set; }
        public string AnomalousChartColor { get; set; }
        public string YAxisTitle { get; set; }
        public bool ReverseYAxisScale { get; set; }
    }

}