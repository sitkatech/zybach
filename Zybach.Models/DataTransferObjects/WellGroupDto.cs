using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public partial class WellGroupDto
{
    public WellSimpleDto PrimaryWell { get; set; }
    public List<WellGroupWellSimpleDto> WellGroupWells { get; set; }
    public bool HasWaterLevelInspections { get; set; }
    public DateTime? LatestWaterLevelInspectionDate { get; set; }
}