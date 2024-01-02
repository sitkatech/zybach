using System.Security.AccessControl;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class WellGroupWellExtensionMethods
{
    static partial void DoCustomSimpleDtoMappings(WellGroupWell wellGroupWell, WellGroupWellSimpleDto wellGroupWellSimpleDto)
    {
        wellGroupWellSimpleDto.WellRegistrationID = wellGroupWell.Well.WellRegistrationID;
        wellGroupWellSimpleDto.Latitude = wellGroupWell.Well.Latitude;
        wellGroupWellSimpleDto.Longitude = wellGroupWell.Well.Longitude;
    }
}