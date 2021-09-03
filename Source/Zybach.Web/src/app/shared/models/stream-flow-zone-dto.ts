import {Feature} from "geojson"

export class StreamFlowZoneDto {
    StreamFlowZoneID: number;
    StreamFlowZoneName: string;
    StreamFlowZoneFeature: Feature;
    StreamFlowZoneArea: number;
}