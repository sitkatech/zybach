//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationInspectionExtensionMethods
    {
        public static ChemigationInspectionDto AsDto(this ChemigationInspection chemigationInspection)
        {
            var chemigationInspectionDto = new ChemigationInspectionDto()
            {
                ChemigationInspectionID = chemigationInspection.ChemigationInspectionID,
                WellRegistrationID = chemigationInspection.WellRegistrationID,
                ProtocolCanonicalName = chemigationInspection.ProtocolCanonicalName,
                Status = chemigationInspection.Status,
                LastUpdate = chemigationInspection.LastUpdate
            };
            DoCustomMappings(chemigationInspection, chemigationInspectionDto);
            return chemigationInspectionDto;
        }

        static partial void DoCustomMappings(ChemigationInspection chemigationInspection, ChemigationInspectionDto chemigationInspectionDto);

    }
}