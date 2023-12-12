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
        public ChemigationInterlockTypeDto ChemigationInterlockType { get; set; }
        public TillageDto Tillage { get; set; }
        public CropTypeDto CropType { get; set; }
        public string InspectionNotes { get; set; }
        public ChemigationInspectionFailureReasonDto ChemigationInspectionFailureReason { get; set; }
    }

    public partial class ChemigationInspectionSimpleDto
    {
        public int ChemigationInspectionID { get; set; }
        public System.Int32 ChemigationPermitAnnualRecordID { get; set; }
        public System.Int32 ChemigationInspectionStatusID { get; set; }
        public System.Int32? ChemigationInspectionTypeID { get; set; }
        public DateTime? InspectionDate { get; set; }
        public System.Int32? InspectorUserID { get; set; }
        public System.Int32? ChemigationMainlineCheckValveID { get; set; }
        public bool? HasVacuumReliefValve { get; set; }
        public bool? HasInspectionPort { get; set; }
        public System.Int32? ChemigationLowPressureValveID { get; set; }
        public System.Int32? ChemigationInjectionValveID { get; set; }
        public System.Int32? ChemigationInterlockTypeID { get; set; }
        public System.Int32? TillageID { get; set; }
        public System.Int32? CropTypeID { get; set; }
        public string InspectionNotes { get; set; }
        public System.Int32? ChemigationInspectionFailureReasonID { get; set; }
    }

}