using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WellExtensionMethods
    {
        static partial void DoCustomMappings(Well well, WellDto wellDto)
        {
            wellDto.Longitude = well.WellGeometry.Coordinate.X;
            wellDto.Latitude = well.WellGeometry.Coordinate.Y;
        }
    }
}
