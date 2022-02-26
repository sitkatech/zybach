namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorAnomalySimpleDto
    {
        public string SensorName { get; set; }
        public string WellRegistrationID { get; set; }
        public int? NumberOfAnomalousDays { get; set; }
    }
}