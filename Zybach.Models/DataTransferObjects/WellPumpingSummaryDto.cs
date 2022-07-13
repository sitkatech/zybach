namespace Zybach.Models.DataTransferObjects;

public class WellPumpingSummaryDto
{
    public int WellID { get; set; }
    public string WellRegistrationID { get; set; }
    public string OwnerName { get; set; }
    public int? MostRecentSupportTicketID { get; set; }
    public string MostRecentSupportTicketTitle { get; set; }
    public string FlowMeters { get; set; }
    public string ContinuityMeters { get; set; }
    public string ElectricalUsage { get; set; }
    public double? FlowMeterPumpedVolumeGallons { get; set; }
    public double? FlowMeterPumpedDepthInches { get; set; }
    public double? ContinuityMeterPumpedVolumeGallons { get; set; }
    public double? ContinuityMeterPumpedDepthInches { get; set; }
    public double? ElectricalUsagePumpedVolumeGallons { get; set; }
    public double? ElectricalUsagePumpedDepthInches { get; set; }
    public double? FlowMeterContinuityMeterDifferenceGallons => FlowMeterPumpedVolumeGallons - ContinuityMeterPumpedVolumeGallons;
    public double? FlowMeterContinuityMeterDifferenceInches => FlowMeterPumpedDepthInches - ContinuityMeterPumpedDepthInches;
    public double? FlowMeterElectricalUsageDifferenceGallons => FlowMeterPumpedVolumeGallons - ElectricalUsagePumpedVolumeGallons;
    public double? FlowMeterElectricalUsageDifferenceInches => FlowMeterPumpedDepthInches - ElectricalUsagePumpedDepthInches;



}