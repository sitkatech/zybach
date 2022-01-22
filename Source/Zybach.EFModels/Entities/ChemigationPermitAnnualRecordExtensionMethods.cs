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
                TownshipRangeSection = chemigationPermitAnnualRecord.TownshipRangeSection,
                ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID,
                ChemigationPermitAnnualRecordStatusName = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusDisplayName,
                PivotName = chemigationPermitAnnualRecord.PivotName,
                ChemigationInjectionUnitTypeID = chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID,
                ChemigationInjectionUnitTypeName = chemigationPermitAnnualRecord.ChemigationInjectionUnitType.ChemigationInjectionUnitTypeDisplayName,
                ChemigationPermitAnnualRecordFeeTypeID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordFeeTypeID,
                ChemigationPermitAnnualRecordFeeTypeName = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordFeeType?.ChemigationPermitAnnualRecordFeeTypeDisplayName,
                ApplicantName = chemigationPermitAnnualRecord.ApplicantName,
                ApplicantFirstName = chemigationPermitAnnualRecord.ApplicantFirstName,
                ApplicantLastName = chemigationPermitAnnualRecord.ApplicantLastName,
                ApplicantCompany = chemigationPermitAnnualRecord.ApplicantCompany,
                ApplicantMailingAddress = chemigationPermitAnnualRecord.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitAnnualRecord.ApplicantCity,
                ApplicantState = chemigationPermitAnnualRecord.ApplicantState,
                ApplicantZipCode = chemigationPermitAnnualRecord.ApplicantZipCode,
                ApplicantPhone = chemigationPermitAnnualRecord.ApplicantPhone,
                ApplicantMobilePhone = chemigationPermitAnnualRecord.ApplicantMobilePhone,
                DateReceived = chemigationPermitAnnualRecord.DateReceived,
                DatePaid = chemigationPermitAnnualRecord.DatePaid,
                DateApproved = chemigationPermitAnnualRecord.DateApproved,
                ApplicantEmail = chemigationPermitAnnualRecord.ApplicantEmail,
                NDEEAmount = chemigationPermitAnnualRecord.NDEEAmount,
                AnnualNotes = chemigationPermitAnnualRecord.AnnualNotes,
                ChemicalFormulations = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordChemicalFormulations?.OrderBy(x => x.ChemicalFormulation.ChemicalFormulationDisplayName).ThenBy(x => x.ChemicalUnit.ChemicalUnitPluralName).Select(x => x.AsSimpleDto()).ToList(),
                Applicators = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordApplicators?.OrderBy(x => x.ApplicatorName).Select(x => x.AsSimpleDto()).ToList(),
                Inspections = chemigationPermitAnnualRecord.ChemigationInspections?.OrderBy(x => x.InspectionDate).Select(x => x.AsSimpleDto()).ToList(),
            };
            return chemigationPermitAnnualRecordDetailedDto;
        }
    }
}