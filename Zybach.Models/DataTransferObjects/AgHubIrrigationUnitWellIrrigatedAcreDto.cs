using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public class AgHubIrrigationUnitFarmingPracticeDto
{
    public int AgHubIrrigationUnitID { get; set; }
    public string WellTPID { get; set; }
    public int IrrigationYear { get; set; }
    public double? Acres { get; set; }

    public string CropType { get; set; }
    public string CropTypeLegendDisplayName { get; set; }
    public string CropTypeMapColor { get; set; }
    public int CropTypeSortOrder { get; set; }

    public string Tillage { get; set; }
    public string TillageTypeLegendDisplayName { get; set; }
    public string TillageTypeMapColor { get; set; }
    public int TillageTypeSortOrder { get; set; }

    public List<WellLinkDto> Wells { get; set; }
}