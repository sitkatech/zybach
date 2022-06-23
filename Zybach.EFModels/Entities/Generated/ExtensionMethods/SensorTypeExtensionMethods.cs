//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SensorType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SensorTypeExtensionMethods
    {
        public static SensorTypeDto AsDto(this SensorType sensorType)
        {
            var sensorTypeDto = new SensorTypeDto()
            {
                SensorTypeID = sensorType.SensorTypeID,
                SensorTypeName = sensorType.SensorTypeName,
                SensorTypeDisplayName = sensorType.SensorTypeDisplayName,
                ChartColor = sensorType.ChartColor,
                AnomalousChartColor = sensorType.AnomalousChartColor,
                YAxisTitle = sensorType.YAxisTitle,
                ReverseYAxisScale = sensorType.ReverseYAxisScale
            };
            DoCustomMappings(sensorType, sensorTypeDto);
            return sensorTypeDto;
        }

        static partial void DoCustomMappings(SensorType sensorType, SensorTypeDto sensorTypeDto);

        public static SensorTypeSimpleDto AsSimpleDto(this SensorType sensorType)
        {
            var sensorTypeSimpleDto = new SensorTypeSimpleDto()
            {
                SensorTypeID = sensorType.SensorTypeID,
                SensorTypeName = sensorType.SensorTypeName,
                SensorTypeDisplayName = sensorType.SensorTypeDisplayName,
                ChartColor = sensorType.ChartColor,
                AnomalousChartColor = sensorType.AnomalousChartColor,
                YAxisTitle = sensorType.YAxisTitle,
                ReverseYAxisScale = sensorType.ReverseYAxisScale
            };
            DoCustomSimpleDtoMappings(sensorType, sensorTypeSimpleDto);
            return sensorTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SensorType sensorType, SensorTypeSimpleDto sensorTypeSimpleDto);
    }
}