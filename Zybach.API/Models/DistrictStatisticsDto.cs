namespace Zybach.API.Models
{
    public class DistrictStatisticsDto
    {
        public int NumberOfWellsTracked { get; set; }
        public int NumberOfContinuityMeters { get; set; }
        public int NumberOfElectricalUsageEstimates { get; set; }
        public int NumberOfAgHubWellsWithConnectedElectricalMeters { get; set; }
        public int NumberOfFlowMeters { get; set; }
    }
}