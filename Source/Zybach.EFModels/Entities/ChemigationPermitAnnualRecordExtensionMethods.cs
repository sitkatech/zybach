using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecordExtensionMethods
    {
        public static ChemigationPermitAnnualRecordDetailedDto AsDetailedDto(this ChemigationPermitAnnualRecord chemigationPermitAnnualRecord)
        {
            var chemigationPermitAnnualRecordDetailedDto = new ChemigationPermitAnnualRecordDetailedDto()
            {
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                ChemigationPermit = chemigationPermitAnnualRecord.ChemigationPermit.AsDto(),
                RecordYear = chemigationPermitAnnualRecord.RecordYear,
                ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID,
                ChemigationPermitAnnualRecordStatusName = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName,
                PivotName = chemigationPermitAnnualRecord.PivotName,
                ChemigationInjectionUnitTypeID = chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID,
                ChemigationInjectionUnitTypeName = chemigationPermitAnnualRecord.ChemigationInjectionUnitType.ChemigationInjectionUnitTypeDisplayName,
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
                ChemicalFormulations = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordChemicalFormulations?.Select(x => x.AsSimpleDto()).ToList(),
                Applicators = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators?.Select(x => x.AsSimpleDto()).ToList()
            };
            return chemigationPermitAnnualRecordDetailedDto;
        }
    }
}