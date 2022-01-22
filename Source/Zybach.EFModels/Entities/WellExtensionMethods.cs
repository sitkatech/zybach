using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WellExtensionMethods
    {
        static partial void DoCustomMappings(Well well, WellDto wellDto)
        {
            wellDto.Longitude = well.Longitude;
            wellDto.Latitude = well.Latitude;
        }

        static partial void DoCustomSimpleDtoMappings(Well well, WellSimpleDto wellSimpleDto)
        {
            wellSimpleDto.WellParticipationName = well.WellParticipation?.WellParticipationDisplayName;
        }
    }
}
