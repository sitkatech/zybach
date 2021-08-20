using NetTopologySuite.Features;
using Newtonsoft.Json.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class StreamFlowZoneExtensionMethods
    {
        static partial void DoCustomMappings(StreamFlowZone streamFlowZone, StreamFlowZoneDto streamFlowZoneDto)
        {
            var geoJsonWriter = new NetTopologySuite.IO.GeoJsonWriter();

            var attributesTable = new AttributesTable();

            attributesTable.Add("FeatureID", streamFlowZone.StreamFlowZoneID);
            var write = geoJsonWriter.Write(new Feature(streamFlowZone.StreamFlowZoneGeometry, attributesTable));
            var jObject = JObject.Parse(write);

            var feature = jObject.ToObject<GeoJSON.Net.Feature.Feature>();
            streamFlowZoneDto.StreamFlowZoneFeature = feature;
        }
    }
}