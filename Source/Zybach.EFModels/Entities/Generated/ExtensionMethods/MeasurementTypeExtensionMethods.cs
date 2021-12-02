//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[MeasurementType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class MeasurementTypeExtensionMethods
    {
        public static MeasurementTypeDto AsDto(this MeasurementType measurementType)
        {
            var measurementTypeDto = new MeasurementTypeDto()
            {
                MeasurementTypeID = measurementType.MeasurementTypeID,
                MeasurementTypeName = measurementType.MeasurementTypeName,
                MeasurementTypeDisplayName = measurementType.MeasurementTypeDisplayName
            };
            DoCustomMappings(measurementType, measurementTypeDto);
            return measurementTypeDto;
        }

        static partial void DoCustomMappings(MeasurementType measurementType, MeasurementTypeDto measurementTypeDto);

        public static MeasurementTypeSimpleDto AsSimpleDto(this MeasurementType measurementType)
        {
            var measurementTypeSimpleDto = new MeasurementTypeSimpleDto()
            {
                MeasurementTypeID = measurementType.MeasurementTypeID,
                MeasurementTypeName = measurementType.MeasurementTypeName,
                MeasurementTypeDisplayName = measurementType.MeasurementTypeDisplayName
            };
            DoCustomSimpleDtoMappings(measurementType, measurementTypeSimpleDto);
            return measurementTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(MeasurementType measurementType, MeasurementTypeSimpleDto measurementTypeSimpleDto);
    }
}