using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateChemigationPermitDetailedModel : ReportTemplateBaseModel
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public string ChemigationPermitNumber { get; set; }
        public string TownshipRangeSection { get; set; }
        public string County { get; set; }
        public int RecordYear { get; set; }
        public string PivotName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantMailingAddress { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZipCode { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantMobilePhone { get; set; }
        public string ApplicantEmail { get; set; }
        public string WellName { get; set; }
        public string WellLatitude { get; set; }
        public string WellLongitude { get; set; }

        public List<ChemigationPermitAnnualRecordApplicatorSimpleDto> Applicators { get; set; }

        public string LastInspectionDate { get; set; }
        public string ChemigationMainlineCheckValveName { get; set; }
        public string ChemigationLowPressureValveName { get; set; }
        public string ChemigationInjectionValveName { get; set; }
        public string ChemigationInterlockTypeName { get; set; }
        public string VacuumReliefValve { get; set;  }
        public string InspectionPort { get; set; }
        public string TillageName { get; set; }
        public string CropTypeName { get; set; }

        public ReportTemplateChemigationPermitDetailedModel(ChemigationPermitDetailedDto chemigationPermit)
        {
            ChemigationPermitAnnualRecordID = chemigationPermit.LatestAnnualRecord.ChemigationPermitAnnualRecordID;
            ChemigationPermitNumber = chemigationPermit.LatestAnnualRecord.ChemigationPermit.ChemigationPermitNumberDisplay;
            TownshipRangeSection = chemigationPermit.LatestAnnualRecord.TownshipRangeSection;
            RecordYear = chemigationPermit.LatestAnnualRecord.RecordYear;
            PivotName = chemigationPermit.LatestAnnualRecord.PivotName;
            ApplicantName = chemigationPermit.LatestAnnualRecord.ApplicantName;
            ApplicantMailingAddress = chemigationPermit.LatestAnnualRecord.ApplicantMailingAddress;
            ApplicantCity = chemigationPermit.LatestAnnualRecord.ApplicantCity;
            ApplicantState = chemigationPermit.LatestAnnualRecord.ApplicantState;
            ApplicantZipCode = chemigationPermit.LatestAnnualRecord.ApplicantZipCode;
            ApplicantPhone = chemigationPermit.LatestAnnualRecord.ApplicantPhone;
            ApplicantMobilePhone = chemigationPermit.LatestAnnualRecord.ApplicantMobilePhone;
            ApplicantEmail = chemigationPermit.LatestAnnualRecord.ApplicantEmail;
            Applicators = chemigationPermit.LatestAnnualRecord.Applicators.ToList();
            County = chemigationPermit.LatestAnnualRecord.ChemigationPermit.County.CountyDisplayName;
            WellName = chemigationPermit.LatestAnnualRecord.ChemigationPermit.Well.WellRegistrationID;
            WellLatitude = chemigationPermit.LatestAnnualRecord.ChemigationPermit.Well.Latitude.ToString("F4");
            WellLongitude = chemigationPermit.LatestAnnualRecord.ChemigationPermit.Well.Longitude.ToString("F4");
            if (chemigationPermit.LatestInspection != null)
            {
                LastInspectionDate = chemigationPermit.LatestInspection.InspectionDate.HasValue
                    ? chemigationPermit.LatestInspection.InspectionDate.Value.ToShortDateString()
                    : "N/A";
                ChemigationMainlineCheckValveName = chemigationPermit.LatestInspection.ChemigationMainlineCheckValveName;
                ChemigationLowPressureValveName = chemigationPermit.LatestInspection.ChemigationLowPressureValveName;
                ChemigationInjectionValveName = chemigationPermit.LatestInspection.ChemigationInjectionValveName;
                ChemigationInterlockTypeName = chemigationPermit.LatestInspection.ChemigationInterlockTypeName;
                VacuumReliefValve = chemigationPermit.LatestInspection.HasVacuumReliefValve.HasValue
                    ? chemigationPermit.LatestInspection.HasVacuumReliefValve.Value ? "Yes" : "No"
                    : "N/A";
                InspectionPort = chemigationPermit.LatestInspection.HasInspectionPort.HasValue
                    ? chemigationPermit.LatestInspection.HasInspectionPort.Value ? "Yes" : "No"
                    : "N/A";
                TillageName = chemigationPermit.LatestInspection.TillageName;
                CropTypeName = chemigationPermit.LatestInspection.CropTypeName;
            }
        }

        public List<ReportTemplateApplicatorModel> GetApplicators()
        {
            return Applicators.Select(x => new ReportTemplateApplicatorModel(x)).OrderBy(x => x.ApplicatorName).ToList();
        }
    }
}
