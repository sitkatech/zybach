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

export class SensorAnomalySimpleDto { 
    SensorAnomalyID?: number;
    SensorID?: number;
    StartDate?: string;
    EndDate?: string;
    Notes?: string;
    SensorName?: string;
    WellRegistrationID?: string;
    NumberOfAnomalousDays?: number;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}