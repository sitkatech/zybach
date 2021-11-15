//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationCounty]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationCountyExtensionMethods
    {
        public static ChemigationCountyDto AsDto(this ChemigationCounty chemigationCounty)
        {
            var chemigationCountyDto = new ChemigationCountyDto()
            {
                ChemigationCountyID = chemigationCounty.ChemigationCountyID,
                ChemigationCountyName = chemigationCounty.ChemigationCountyName,
                ChemigationCountyDisplayName = chemigationCounty.ChemigationCountyDisplayName
            };
            DoCustomMappings(chemigationCounty, chemigationCountyDto);
            return chemigationCountyDto;
        }

        static partial void DoCustomMappings(ChemigationCounty chemigationCounty, ChemigationCountyDto chemigationCountyDto);

    }
}