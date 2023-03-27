using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class WellGroupExtensionMethods
{
    static partial void DoCustomMappings(WellGroup wellGroup, WellGroupDto wellGroupDto)
    {
        wellGroupDto.PrimaryWell = wellGroup.WellGroupWells.SingleOrDefault(x => x.IsPrimary)?.Well.AsSimpleDto();
        wellGroupDto.WellGroupWells = wellGroup.WellGroupWells.Select(x => x.AsSimpleDto()).ToList();
    }
}