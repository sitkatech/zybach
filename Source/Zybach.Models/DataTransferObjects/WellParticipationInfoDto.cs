namespace Zybach.Models.DataTransferObjects
{
    public class WellParticipationInfoDto
    {
        public int? WellParticipationID { get; set; }
        public string? WellParticipationName { get; set; }
        public int? WellUseID { get; set; }
        public string? WellUseName { get; set; }
        public bool RequiresChemigation { get; set; }
        public bool RequiresWaterLevelInspection { get; set; }
        public bool IsReplacement { get; set; }
        public decimal? WellDepth { get; set; }
        public string? Clearinghouse { get; set; }
        public int? PageNumber { get; set; }
        public string? SiteName { get; set; }
        public string? SiteNumber { get; set; }
    }
}
