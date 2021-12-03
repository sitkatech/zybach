//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordWell]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordWellExtensionMethods
    {
        public static ChemigationPermitAnnualRecordWellDto AsDto(this ChemigationPermitAnnualRecordWell chemigationPermitAnnualRecordWell)
        {
            var chemigationPermitAnnualRecordWellDto = new ChemigationPermitAnnualRecordWellDto()
            {
                ChemigationPermitAnnualRecordWellID = chemigationPermitAnnualRecordWell.ChemigationPermitAnnualRecordWellID,
                ChemigationPermitAnnualRecord = chemigationPermitAnnualRecordWell.ChemigationPermitAnnualRecord.AsDto(),
                Well = chemigationPermitAnnualRecordWell.Well.AsDto()
            };
            DoCustomMappings(chemigationPermitAnnualRecordWell, chemigationPermitAnnualRecordWellDto);
            return chemigationPermitAnnualRecordWellDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecordWell chemigationPermitAnnualRecordWell, ChemigationPermitAnnualRecordWellDto chemigationPermitAnnualRecordWellDto);

        public static ChemigationPermitAnnualRecordWellSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecordWell chemigationPermitAnnualRecordWell)
        {
            var chemigationPermitAnnualRecordWellSimpleDto = new ChemigationPermitAnnualRecordWellSimpleDto()
            {
                ChemigationPermitAnnualRecordWellID = chemigationPermitAnnualRecordWell.ChemigationPermitAnnualRecordWellID,
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecordWell.ChemigationPermitAnnualRecordID,
                WellID = chemigationPermitAnnualRecordWell.WellID
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecordWell, chemigationPermitAnnualRecordWellSimpleDto);
            return chemigationPermitAnnualRecordWellSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordWell chemigationPermitAnnualRecordWell, ChemigationPermitAnnualRecordWellSimpleDto chemigationPermitAnnualRecordWellSimpleDto);
    }
}