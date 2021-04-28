import {Geometry} from "geojson"

export class StreamFlowZoneDto {
    type: string;
    properties: {
        FeatureID: number,
        ZoneName: string,
        Length: number,
        Area: number
    };
    geometry: Geometry;
}