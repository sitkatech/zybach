using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class WellGroupWellExtensionMethods
{
    static partial void DoCustomSimpleDtoMappings(WellGroupWell wellGroupWell, WellGroupWellSimpleDto wellGroupWellSimpleDto)
    {
        wellGroupWellSimpleDto.WellRegistrationID = wellGroupWell.Well.WellRegistrationID;
    }
}