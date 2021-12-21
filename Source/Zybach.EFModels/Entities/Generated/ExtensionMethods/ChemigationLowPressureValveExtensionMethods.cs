//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationLowPressureValve]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationLowPressureValveExtensionMethods
    {
        public static ChemigationLowPressureValveDto AsDto(this ChemigationLowPressureValve chemigationLowPressureValve)
        {
            var chemigationLowPressureValveDto = new ChemigationLowPressureValveDto()
            {
                ChemigationLowPressureValveID = chemigationLowPressureValve.ChemigationLowPressureValveID,
                ChemigationLowPressureValveName = chemigationLowPressureValve.ChemigationLowPressureValveName,
                ChemigationLowPressureValveDisplayName = chemigationLowPressureValve.ChemigationLowPressureValveDisplayName
            };
            DoCustomMappings(chemigationLowPressureValve, chemigationLowPressureValveDto);
            return chemigationLowPressureValveDto;
        }

        static partial void DoCustomMappings(ChemigationLowPressureValve chemigationLowPressureValve, ChemigationLowPressureValveDto chemigationLowPressureValveDto);

        public static ChemigationLowPressureValveSimpleDto AsSimpleDto(this ChemigationLowPressureValve chemigationLowPressureValve)
        {
            var chemigationLowPressureValveSimpleDto = new ChemigationLowPressureValveSimpleDto()
            {
                ChemigationLowPressureValveID = chemigationLowPressureValve.ChemigationLowPressureValveID,
                ChemigationLowPressureValveName = chemigationLowPressureValve.ChemigationLowPressureValveName,
                ChemigationLowPressureValveDisplayName = chemigationLowPressureValve.ChemigationLowPressureValveDisplayName
            };
            DoCustomSimpleDtoMappings(chemigationLowPressureValve, chemigationLowPressureValveSimpleDto);
            return chemigationLowPressureValveSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationLowPressureValve chemigationLowPressureValve, ChemigationLowPressureValveSimpleDto chemigationLowPressureValveSimpleDto);
    }
}