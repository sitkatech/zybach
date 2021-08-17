//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[StreamFlowZone]

using GeoJSON.Net.Feature;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;


namespace Zybach.Models.DataTransferObjects
{
    
    public partial class StreamFlowZoneDto
    {
        public int StreamFlowZoneID { get; set; }
        public string StreamFlowZoneName { get; set; }
        public Feature StreamFlowZoneFeature { get; set; }
    }
}