/**
 * Zybach
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

export class InstallationRecordDto { 
    InstallationCanonicalName?: string;
    Status?: string;
    Date?: string;
    Longitude?: number;
    Latitude?: number;
    FlowMeterSerialNumber?: string;
    SensorSerialNumber?: string;
    Affiliation?: string;
    Initials?: string;
    SensorType?: string;
    ContinuitySensorModel?: string;
    FlowSensorModel?: string;
    PressureSensorModel?: string;
    WellDepth?: number;
    InstallDepth?: number;
    CableLength?: number;
    WaterLevel?: number;
    Photos?: Array<string>;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}