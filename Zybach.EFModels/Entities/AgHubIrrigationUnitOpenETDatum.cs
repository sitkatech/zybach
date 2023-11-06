namespace Zybach.EFModels.Entities;

public partial class AgHubIrrigationUnitOpenETDatum
{
    public decimal? ReportedValueAcreInches => ReportedValueInches * AgHubIrrigationUnitAreaInAcres;
}