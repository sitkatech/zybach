namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationInspectionSimpleDto
    {
        public int ChemigationPermitNumber { get; set; }
        public string ChemigationPermitNumberDisplay { get; set; }
        public string County { get; set; }
        public string TownshipRangeSection { get; set; }
        public string ChemigationInspectionFailureReasonName { get; set; }
        public string ChemigationInspectionTypeName { get; set; }
        public string ChemigationInspectionStatusName { get; set; }
        public string ChemigationMainlineCheckValveName { get; set; }
        public string ChemigationLowPressureValveName { get; set; }
        public string ChemigationInjectionValveName { get; set; }
        public string ChemigationInterlockTypeName { get; set; }
        public string TillageName { get; set; }
        public string CropTypeName { get; set; }
        public UserSimpleDto Inspector { get; set; }
    }
}