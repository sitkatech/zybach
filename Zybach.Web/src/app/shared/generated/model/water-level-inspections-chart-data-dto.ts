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
import { WaterLevelInspectionSummaryDto } from '././water-level-inspection-summary-dto';

export class WaterLevelInspectionsChartDataDto { 
    WaterLevelInspections?: Array<WaterLevelInspectionSummaryDto>;
    LastInspectionDate?: string;
    ChartSpec?: string;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}