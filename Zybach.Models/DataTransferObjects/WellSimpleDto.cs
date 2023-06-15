using GeoJSON.Net.Feature;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSimpleDto
    {
        public string WellParticipationName { get; set; }
        public Feature Location { get; set; }
    }
}