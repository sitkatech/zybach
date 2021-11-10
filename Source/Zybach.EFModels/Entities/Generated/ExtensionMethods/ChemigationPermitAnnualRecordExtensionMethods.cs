//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordExtensionMethods
    {
        public static ChemigationPermitAnnualRecordDto AsDto(this ChemigationPermitAnnualRecord chemigationPermitAnnualRecord)
        {
            var chemigationPermitAnnualRecordDto = new ChemigationPermitAnnualRecordDto()
            {
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                ChemigationPermit = chemigationPermitAnnualRecord.ChemigationPermit.AsDto(),
                ChemigationPermitAnnualRecordStatus = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatus.AsDto(),
                ApplicantFirstName = chemigationPermitAnnualRecord.ApplicantFirstName,
                ApplicantLastName = chemigationPermitAnnualRecord.ApplicantLastName,
                PivotName = chemigationPermitAnnualRecord.PivotName,
                RecordYear = chemigationPermitAnnualRecord.RecordYear,
                DateReceived = chemigationPermitAnnualRecord.DateReceived,
                DatePaid = chemigationPermitAnnualRecord.DatePaid
            };
            DoCustomMappings(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordDto);
            return chemigationPermitAnnualRecordDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordDto chemigationPermitAnnualRecordDto);

    }
}