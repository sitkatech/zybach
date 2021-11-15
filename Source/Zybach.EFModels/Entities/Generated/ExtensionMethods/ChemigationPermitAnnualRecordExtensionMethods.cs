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
                RecordYear = chemigationPermitAnnualRecord.RecordYear,
                ChemigationPermitAnnualRecordStatus = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatus.AsDto(),
                PivotName = chemigationPermitAnnualRecord.PivotName,
                ApplicantFirstName = chemigationPermitAnnualRecord.ApplicantFirstName,
                ApplicantLastName = chemigationPermitAnnualRecord.ApplicantLastName,
                ApplicantMailingAddress = chemigationPermitAnnualRecord.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitAnnualRecord.ApplicantCity,
                ApplicantState = chemigationPermitAnnualRecord.ApplicantState,
                ApplicantZipCode = chemigationPermitAnnualRecord.ApplicantZipCode,
                DateReceived = chemigationPermitAnnualRecord.DateReceived,
                DatePaid = chemigationPermitAnnualRecord.DatePaid
            };
            DoCustomMappings(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordDto);
            return chemigationPermitAnnualRecordDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordDto chemigationPermitAnnualRecordDto);

    }
}