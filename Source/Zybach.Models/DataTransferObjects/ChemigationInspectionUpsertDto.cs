using System;

namespace Zybach.Models.DataTransferObjects
{
    public class ChemigationInspectionUpsertDto
    {
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int? ChemigationInspectionTypeID { get; set; }
        public int ChemigationInspectionStatusID { get; set; }
        public DateTime? InspectionDate { get; set; }
        public int? ChemigationInspectionFailureReasonID { get; set; }

        public int? TillageID { get; set; }
        public int? CropTypeID { get; set; }

        public int? InspectorUserID { get; set; }

        public int? ChemigationMainlineCheckValveID { get; set; }
        public int? ChemigationLowPressureValveID { get; set; }
        public int? ChemigationInjectionValveID { get; set; }
        public int? ChemigationInterlockTypeID { get; set; }
        public bool? HasVacuumReliefValve { get; set; }
        public bool? HasInspectionPort { get; set; }

        public string InspectionNotes { get; set; }

    }
}