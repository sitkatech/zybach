import {Feature, Geometry} from "geojson"

export class StreamFlowZoneDto {
    StreamFlowZoneID: number;
    StreamFlowZoneName: string;
    StreamFlowZoneFeature: Feature;
    Area;
}