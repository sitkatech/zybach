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
import { MeasurementTypeDto } from '././measurement-type-dto';

export class WellSensorMeasurementDto { 
    WellSensorMeasurementID?: number;
    WellRegistrationID?: string;
    MeasurementType?: MeasurementTypeDto;
    ReadingYear?: number;
    ReadingMonth?: number;
    ReadingDay?: number;
    SensorName?: string;
    MeasurementValue?: number;
    IsAnomalous?: boolean;
    MeasurementDateInPacificTime?: string;
    MeasurementDate?: string;
    MeasurementValueString?: string;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}