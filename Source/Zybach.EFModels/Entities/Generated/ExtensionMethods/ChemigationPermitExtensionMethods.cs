//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermit]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitExtensionMethods
    {
        public static ChemigationPermitDto AsDto(this ChemigationPermit chemigationPermit)
        {
            var chemigationPermitDto = new ChemigationPermitDto()
            {
                ChemigationPermitID = chemigationPermit.ChemigationPermitID,
                ChemigationPermitNumber = chemigationPermit.ChemigationPermitNumber,
                ChemigationPermitStatus = chemigationPermit.ChemigationPermitStatus.AsDto(),
                DateCreated = chemigationPermit.DateCreated,
                ChemigationCounty = chemigationPermit.ChemigationCounty.AsDto(),
                Well = chemigationPermit.Well?.AsDto()
            };
            DoCustomMappings(chemigationPermit, chemigationPermitDto);
            return chemigationPermitDto;
        }

        static partial void DoCustomMappings(ChemigationPermit chemigationPermit, ChemigationPermitDto chemigationPermitDto);

        public static ChemigationPermitSimpleDto AsSimpleDto(this ChemigationPermit chemigationPermit)
        {
            var chemigationPermitSimpleDto = new ChemigationPermitSimpleDto()
            {
                ChemigationPermitID = chemigationPermit.ChemigationPermitID,
                ChemigationPermitNumber = chemigationPermit.ChemigationPermitNumber,
                ChemigationPermitStatusID = chemigationPermit.ChemigationPermitStatusID,
                DateCreated = chemigationPermit.DateCreated,
                ChemigationCountyID = chemigationPermit.ChemigationCountyID,
                WellID = chemigationPermit.WellID
            };
            DoCustomSimpleDtoMappings(chemigationPermit, chemigationPermitSimpleDto);
            return chemigationPermitSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermit chemigationPermit, ChemigationPermitSimpleDto chemigationPermitSimpleDto);
    }
}