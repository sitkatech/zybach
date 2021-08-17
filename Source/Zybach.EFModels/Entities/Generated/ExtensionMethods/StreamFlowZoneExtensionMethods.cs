//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[StreamFlowZone]

using NetTopologySuite.Features;
using Newtonsoft.Json.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class StreamFlowZoneExtensionMethods
    {
        public static StreamFlowZoneDto AsDto(this StreamFlowZone streamFlowZone)
        {
            var geoJsonWriter = new NetTopologySuite.IO.GeoJsonWriter();

            var attributesTable = new AttributesTable();

            attributesTable.Add("FeatureID", streamFlowZone.StreamFlowZoneID);
            var write = geoJsonWriter.Write(new Feature(streamFlowZone.StreamFlowZoneGeometry, attributesTable));
            var jObject = JObject.Parse(write);

            var feature = jObject.ToObject<GeoJSON.Net.Feature.Feature>();


            var streamFlowZoneDto = new StreamFlowZoneDto()
            {
                StreamFlowZoneID = streamFlowZone.StreamFlowZoneID,
                StreamFlowZoneName = streamFlowZone.StreamFlowZoneName,
                StreamFlowZoneFeature = feature
            };
            DoCustomMappings(streamFlowZone, streamFlowZoneDto);
            return streamFlowZoneDto;
        }

        static partial void DoCustomMappings(StreamFlowZone streamFlowZone, StreamFlowZoneDto streamFlowZoneDto);

    }
}