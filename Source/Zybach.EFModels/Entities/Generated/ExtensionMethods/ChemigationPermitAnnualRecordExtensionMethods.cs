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
                ChemigationInjectionUnitType = chemigationPermitAnnualRecord.ChemigationInjectionUnitType.AsDto(),
                ApplicantFirstName = chemigationPermitAnnualRecord.ApplicantFirstName,
                ApplicantLastName = chemigationPermitAnnualRecord.ApplicantLastName,
                ApplicantMailingAddress = chemigationPermitAnnualRecord.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitAnnualRecord.ApplicantCity,
                ApplicantState = chemigationPermitAnnualRecord.ApplicantState,
                ApplicantZipCode = chemigationPermitAnnualRecord.ApplicantZipCode,
                ApplicantPhone = chemigationPermitAnnualRecord.ApplicantPhone,
                ApplicantMobilePhone = chemigationPermitAnnualRecord.ApplicantMobilePhone,
                DateReceived = chemigationPermitAnnualRecord.DateReceived,
                DatePaid = chemigationPermitAnnualRecord.DatePaid,
                ApplicantEmail = chemigationPermitAnnualRecord.ApplicantEmail,
                NDEEAmount = chemigationPermitAnnualRecord.NDEEAmount,
                TownshipRangeSection = chemigationPermitAnnualRecord.TownshipRangeSection,
                ApplicantCompany = chemigationPermitAnnualRecord.ApplicantCompany,
                AnnualNotes = chemigationPermitAnnualRecord.AnnualNotes,
                DateApproved = chemigationPermitAnnualRecord.DateApproved
            };
            DoCustomMappings(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordDto);
            return chemigationPermitAnnualRecordDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordDto chemigationPermitAnnualRecordDto);

        public static ChemigationPermitAnnualRecordSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecord chemigationPermitAnnualRecord)
        {
            var chemigationPermitAnnualRecordSimpleDto = new ChemigationPermitAnnualRecordSimpleDto()
            {
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                ChemigationPermitID = chemigationPermitAnnualRecord.ChemigationPermitID,
                RecordYear = chemigationPermitAnnualRecord.RecordYear,
                ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID,
                PivotName = chemigationPermitAnnualRecord.PivotName,
                ChemigationInjectionUnitTypeID = chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID,
                ApplicantFirstName = chemigationPermitAnnualRecord.ApplicantFirstName,
                ApplicantLastName = chemigationPermitAnnualRecord.ApplicantLastName,
                ApplicantMailingAddress = chemigationPermitAnnualRecord.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitAnnualRecord.ApplicantCity,
                ApplicantState = chemigationPermitAnnualRecord.ApplicantState,
                ApplicantZipCode = chemigationPermitAnnualRecord.ApplicantZipCode,
                ApplicantPhone = chemigationPermitAnnualRecord.ApplicantPhone,
                ApplicantMobilePhone = chemigationPermitAnnualRecord.ApplicantMobilePhone,
                DateReceived = chemigationPermitAnnualRecord.DateReceived,
                DatePaid = chemigationPermitAnnualRecord.DatePaid,
                ApplicantEmail = chemigationPermitAnnualRecord.ApplicantEmail,
                NDEEAmount = chemigationPermitAnnualRecord.NDEEAmount,
                TownshipRangeSection = chemigationPermitAnnualRecord.TownshipRangeSection,
                ApplicantCompany = chemigationPermitAnnualRecord.ApplicantCompany,
                AnnualNotes = chemigationPermitAnnualRecord.AnnualNotes,
                DateApproved = chemigationPermitAnnualRecord.DateApproved
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordSimpleDto);
            return chemigationPermitAnnualRecordSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordSimpleDto chemigationPermitAnnualRecordSimpleDto);
    }
}