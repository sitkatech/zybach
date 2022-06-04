//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[StreamFlowZone]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class StreamFlowZoneExtensionMethods
    {
        public static StreamFlowZoneDto AsDto(this StreamFlowZone streamFlowZone)
        {
            var streamFlowZoneDto = new StreamFlowZoneDto()
            {
                StreamFlowZoneID = streamFlowZone.StreamFlowZoneID,
                StreamFlowZoneName = streamFlowZone.StreamFlowZoneName,
                StreamFlowZoneArea = streamFlowZone.StreamFlowZoneArea
            };
            DoCustomMappings(streamFlowZone, streamFlowZoneDto);
            return streamFlowZoneDto;
        }

        static partial void DoCustomMappings(StreamFlowZone streamFlowZone, StreamFlowZoneDto streamFlowZoneDto);

        public static StreamFlowZoneSimpleDto AsSimpleDto(this StreamFlowZone streamFlowZone)
        {
            var streamFlowZoneSimpleDto = new StreamFlowZoneSimpleDto()
            {
                StreamFlowZoneID = streamFlowZone.StreamFlowZoneID,
                StreamFlowZoneName = streamFlowZone.StreamFlowZoneName,
                StreamFlowZoneArea = streamFlowZone.StreamFlowZoneArea
            };
            DoCustomSimpleDtoMappings(streamFlowZone, streamFlowZoneSimpleDto);
            return streamFlowZoneSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(StreamFlowZone streamFlowZone, StreamFlowZoneSimpleDto streamFlowZoneSimpleDto);
    }
}