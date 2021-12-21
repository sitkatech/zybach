//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationMainlineCheckValve]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationMainlineCheckValveExtensionMethods
    {
        public static ChemigationMainlineCheckValveDto AsDto(this ChemigationMainlineCheckValve chemigationMainlineCheckValve)
        {
            var chemigationMainlineCheckValveDto = new ChemigationMainlineCheckValveDto()
            {
                ChemigationMainlineCheckValveID = chemigationMainlineCheckValve.ChemigationMainlineCheckValveID,
                ChemigationMainlineCheckValveName = chemigationMainlineCheckValve.ChemigationMainlineCheckValveName,
                ChemigationMainlineCheckValveDisplayName = chemigationMainlineCheckValve.ChemigationMainlineCheckValveDisplayName
            };
            DoCustomMappings(chemigationMainlineCheckValve, chemigationMainlineCheckValveDto);
            return chemigationMainlineCheckValveDto;
        }

        static partial void DoCustomMappings(ChemigationMainlineCheckValve chemigationMainlineCheckValve, ChemigationMainlineCheckValveDto chemigationMainlineCheckValveDto);

        public static ChemigationMainlineCheckValveSimpleDto AsSimpleDto(this ChemigationMainlineCheckValve chemigationMainlineCheckValve)
        {
            var chemigationMainlineCheckValveSimpleDto = new ChemigationMainlineCheckValveSimpleDto()
            {
                ChemigationMainlineCheckValveID = chemigationMainlineCheckValve.ChemigationMainlineCheckValveID,
                ChemigationMainlineCheckValveName = chemigationMainlineCheckValve.ChemigationMainlineCheckValveName,
                ChemigationMainlineCheckValveDisplayName = chemigationMainlineCheckValve.ChemigationMainlineCheckValveDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationMainlineCheckValve, chemigationMainlineCheckValveSimpleDto);
            return chemigationMainlineCheckValveSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationMainlineCheckValve chemigationMainlineCheckValve, ChemigationMainlineCheckValveSimpleDto chemigationMainlineCheckValveSimpleDto);
    }
}