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
                ApplicantName = chemigationPermitAnnualRecord.ApplicantName,
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
                ChemicalFormulations = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordChemicalFormulations?.OrderBy(x => x.ChemicalFormulation.ChemicalFormulationDisplayName).ThenBy(x => x.ChemicalUnit.ChemicalUnitPluralName).Select(x => x.AsSimpleDto()).ToList(),
                Applicators = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators?.OrderBy(x => x.ApplicatorName).Select(x => x.AsSimpleDto()).ToList(),
            };
            return chemigationPermitAnnualRecordDetailedDto;
        }
    }
}