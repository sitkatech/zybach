namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorSimpleDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorTypeName { get; set; }
        public int? MessageAge { get; set; }
    }
}
