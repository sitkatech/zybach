export class SearchSummaryDto {
    ObjectName?: string;
    ObjectType?: string;
    WellID?: string;
}

export enum GeoOptixObjectTypeEnum{
    Site = "Site",
    Station = "Station",
    Sample = "Sample"
}

export enum ZybachObjectTypeEnum {
    Well = "Well",
    Sensor = "Sensor",
    Installation = "Installation"
}

export function GeoOptixObjectTypeToZybachObjectType (geoOptixObjectType : string) : string {
    switch (geoOptixObjectType) {
        case GeoOptixObjectTypeEnum.Site:
            return ZybachObjectTypeEnum.Well;
        case GeoOptixObjectTypeEnum.Station:
            return ZybachObjectTypeEnum.Sensor;
        case GeoOptixObjectTypeEnum.Sample:
            return ZybachObjectTypeEnum.Installation;
        default:
            return "";
    }
}
