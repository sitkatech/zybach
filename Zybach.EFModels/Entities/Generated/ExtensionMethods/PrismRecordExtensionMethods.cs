//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismRecord]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismRecordExtensionMethods
    {
        public static PrismRecordDto AsDto(this PrismRecord prismRecord)
        {
            var prismRecordDto = new PrismRecordDto()
            {
                PrismRecordID = prismRecord.PrismRecordID,
                ElementType = prismRecord.ElementType,
                Date = prismRecord.Date,
                X = prismRecord.X,
                Y = prismRecord.Y,
                Value = prismRecord.Value
            };
            DoCustomMappings(prismRecord, prismRecordDto);
            return prismRecordDto;
        }

        static partial void DoCustomMappings(PrismRecord prismRecord, PrismRecordDto prismRecordDto);

        public static PrismRecordSimpleDto AsSimpleDto(this PrismRecord prismRecord)
        {
            var prismRecordSimpleDto = new PrismRecordSimpleDto()
            {
                PrismRecordID = prismRecord.PrismRecordID,
                ElementType = prismRecord.ElementType,
                Date = prismRecord.Date,
                X = prismRecord.X,
                Y = prismRecord.Y,
                Value = prismRecord.Value
            };
            DoCustomSimpleDtoMappings(prismRecord, prismRecordSimpleDto);
            return prismRecordSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismRecord prismRecord, PrismRecordSimpleDto prismRecordSimpleDto);
    }
}