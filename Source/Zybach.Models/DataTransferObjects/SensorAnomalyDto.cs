namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorAnomalySimpleDto
    {
        public SensorSimpleDto Sensor { get; set; }
        public int? NumberOfAnomalousDays { get; set; }
    }
}