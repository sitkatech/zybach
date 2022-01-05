namespace Zybach.Models.DataTransferObjects
{
    public partial class WaterQualityInspectionSimpleDto
    {
        public WellSimpleDto Well { get; set; }
        public string WaterQualityInspectionTypeName { get; set; }
        public string CropTypeName { get; set; }
        public UserSimpleDto Inspector { get; set; }
        public int InspectionYear { get; set; }
    }
}