//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInjectionValve]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInjectionValveExtensionMethods
    {
        public static ChemigationInjectionValveDto AsDto(this ChemigationInjectionValve chemigationInjectionValve)
        {
            var chemigationInjectionValveDto = new ChemigationInjectionValveDto()
            {
                ChemigationInjectionValveID = chemigationInjectionValve.ChemigationInjectionValveID,
                ChemigationInjectionValveName = chemigationInjectionValve.ChemigationInjectionValveName,
                ChemigationInjectionValveDisplayName = chemigationInjectionValve.ChemigationInjectionValveDisplayName
            };
            DoCustomMappings(chemigationInjectionValve, chemigationInjectionValveDto);
            return chemigationInjectionValveDto;
        }

        static partial void DoCustomMappings(ChemigationInjectionValve chemigationInjectionValve, ChemigationInjectionValveDto chemigationInjectionValveDto);

        public static ChemigationInjectionValveSimpleDto AsSimpleDto(this ChemigationInjectionValve chemigationInjectionValve)
        {
            var chemigationInjectionValveSimpleDto = new ChemigationInjectionValveSimpleDto()
            {
                ChemigationInjectionValveID = chemigationInjectionValve.ChemigationInjectionValveID,
                ChemigationInjectionValveName = chemigationInjectionValve.ChemigationInjectionValveName,
                ChemigationInjectionValveDisplayName = chemigationInjectionValve.ChemigationInjectionValveDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationInjectionValve, chemigationInjectionValveSimpleDto);
            return chemigationInjectionValveSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationInjectionValve chemigationInjectionValve, ChemigationInjectionValveSimpleDto chemigationInjectionValveSimpleDto);
    }
}