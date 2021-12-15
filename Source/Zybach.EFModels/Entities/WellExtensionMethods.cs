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
    }
}
