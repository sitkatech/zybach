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

export class WaterQualityInspectionUpsertDto { 
    WellRegistrationID: string;
    WaterQualityInspectionTypeID: number;
    InspectionDate: string;
    InspectorUserID: number;
    Temperature?: number;
    PH?: number;
    Conductivity?: number;
    FieldAlkilinity?: number;
    FieldNitrates?: number;
    LabNitrates?: number;
    Salinity?: number;
    MV?: number;
    Sodium?: number;
    Calcium?: number;
    Magnesium?: number;
    Potassium?: number;
    HydrogenCarbonate?: number;
    CalciumCarbonate?: number;
    Sulfate?: number;
    Chloride?: number;
    SiliconDioxide?: number;
    CropTypeID?: number;
    PreWaterLevel?: number;
    PostWaterLevel?: number;
    InspectionNotes?: string;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}