//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordFeeType]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordFeeTypeExtensionMethods
    {
        public static ChemigationPermitAnnualRecordFeeTypeDto AsDto(this ChemigationPermitAnnualRecordFeeType chemigationPermitAnnualRecordFeeType)
        {
            var chemigationPermitAnnualRecordFeeTypeDto = new ChemigationPermitAnnualRecordFeeTypeDto()
            {
                ChemigationPermitAnnualRecordFeeTypeID = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeID,
                ChemigationPermitAnnualRecordFeeTypeName = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeName,
                ChemigationPermitAnnualRecordFeeTypeDisplayName = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeDisplayName,
                FeeAmount = chemigationPermitAnnualRecordFeeType.FeeAmount
            };
            DoCustomMappings(chemigationPermitAnnualRecordFeeType, chemigationPermitAnnualRecordFeeTypeDto);
            return chemigationPermitAnnualRecordFeeTypeDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecordFeeType chemigationPermitAnnualRecordFeeType, ChemigationPermitAnnualRecordFeeTypeDto chemigationPermitAnnualRecordFeeTypeDto);

        public static ChemigationPermitAnnualRecordFeeTypeSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecordFeeType chemigationPermitAnnualRecordFeeType)
        {
            var chemigationPermitAnnualRecordFeeTypeSimpleDto = new ChemigationPermitAnnualRecordFeeTypeSimpleDto()
            {
                ChemigationPermitAnnualRecordFeeTypeID = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeID,
                ChemigationPermitAnnualRecordFeeTypeName = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeName,
                ChemigationPermitAnnualRecordFeeTypeDisplayName = chemigationPermitAnnualRecordFeeType.ChemigationPermitAnnualRecordFeeTypeDisplayName,
                FeeAmount = chemigationPermitAnnualRecordFeeType.FeeAmount
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecordFeeType, chemigationPermitAnnualRecordFeeTypeSimpleDto);
            return chemigationPermitAnnualRecordFeeTypeSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordFeeType chemigationPermitAnnualRecordFeeType, ChemigationPermitAnnualRecordFeeTypeSimpleDto chemigationPermitAnnualRecordFeeTypeSimpleDto);
    }
}