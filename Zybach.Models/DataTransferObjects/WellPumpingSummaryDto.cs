namespace Zybach.Models.DataTransferObjects;

public class WellPumpingSummaryDto
{
    public int WellID { get; set; }
    public string WellRegistrationID { get; set; }
    public string OwnerName { get; set; }
    public int? MostRecentSupportTicketID { get; set; }
    public string MostRecentSupportTicketTitle { get; set; }
    public bool HasFlowMeter { get; set; }
    public bool HasContinuityMeter { get; set; }
    public bool HasElectricalUsage { get; set; }
    public double? FlowMeterPumpedVolume { get; set; }
    public double? ContinuityMeterPumpedVolume { get; set; }
    public double? ElectricalUsagePumpedVolume { get; set; }
    public double? FlowMeterContinuityMeterDifference { get; set; }
    public double? FlowMeterElectricalUsageDifference { get; set; }
}