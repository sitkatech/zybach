using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubWellExtensionMethods
    {
        static partial void DoCustomMappings(AgHubWell agHubWell, AgHubWellDto agHubWellDto)
        {
            agHubWellDto.Longitude = agHubWell.WellGeometry.Coordinate.X;
            agHubWellDto.Latitude = agHubWell.WellGeometry.Coordinate.Y;
        }
    }
}
