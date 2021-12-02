namespace Zybach.EFModels.Entities
{
    public partial class AgHubWell
    {
        public int PumpingRateGallonsPerMinute => WellAuditPumpRate ?? RegisteredPumpRate ?? WellTPNRDPumpRate ?? 0;
    }
}