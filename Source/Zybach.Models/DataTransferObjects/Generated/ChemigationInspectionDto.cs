//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationInspectionDto
    {
        public int ChemigationInspectionID { get; set; }
        public ChemigationPermitAnnualRecordDto ChemigationPermitAnnualRecord { get; set; }
        public ChemigationInspectionStatusDto ChemigationInspectionStatus { get; set; }
        public ChemigationInspectionTypeDto ChemigationInspectionType { get; set; }
        public DateTime? InspectionDate { get; set; }
        public UserDto InspectorUser { get; set; }
        public ChemigationMainlineCheckValveDto ChemigationMainlineCheckValve { get; set; }
        public bool? HasVacuumReliefValve { get; set; }
        public bool? HasInspectionPort { get; set; }
        public ChemigationLowPressureValveDto ChemigationLowPressureValve { get; set; }
        public ChemigationInjectionValveDto ChemigationInjectionValve { get; set; }
        public TillageDto Tillage { get; set; }
        public CropTypeDto CropType { get; set; }
        public string InspectionNotes { get; set; }
    }

    public partial class ChemigationInspectionSimpleDto
    {
        public int ChemigationInspectionID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemigationInspectionStatusID { get; set; }
        public int? ChemigationInspectionTypeID { get; set; }
        public DateTime? InspectionDate { get; set; }
        public int? InspectorUserID { get; set; }
        public int? ChemigationMainlineCheckValveID { get; set; }
        public bool? HasVacuumReliefValve { get; set; }
        public bool? HasInspectionPort { get; set; }
        public int? ChemigationLowPressureValveID { get; set; }
        public int? ChemigationInjectionValveID { get; set; }
        public int? TillageID { get; set; }
        public int? CropTypeID { get; set; }
        public string InspectionNotes { get; set; }
    }

}