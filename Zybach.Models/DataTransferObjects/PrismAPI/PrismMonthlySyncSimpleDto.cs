namespace Zybach.Models.DataTransferObjects;

public partial class PrismMonthlySyncSimpleDto
{
    public string PrismDataTypeName { get; set; }
    public string PrismDataTypeDisplayName { get; set; }

    public string PrismSyncStatusName { get; set; }
    public string PrismSyncStatusDisplayName { get; set; }
    public string RunoffCalculationStatusName { get; set; }
    public string RunoffCalculationStatusDisplayName { get; set; }

    public string LastSynchronizedByUserFullName { get; set; }
    public string LastRunoffCalculatedByUserFullName { get; set; }
    public string FinalizedByUserFullName { get; set; }

}

public partial class PrismDailyRecordSimpleDto
{
    public string BlobResourceCanonicalName { get; set; }
    public string BlobFileName { get; set; }
}

public partial class AgHubIrrigationUnitRunoffSimpleDto
{
    public string WellTPID { get; set; }
}