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
import { ZybachModelsDataTransferObjectsDailyPumpedVolume } from '././zybach-models-data-transfer-objects-daily-pumped-volume';
import { ZybachModelsDataTransferObjectsSensorSummaryDto } from '././zybach-models-data-transfer-objects-sensor-summary-dto';

export class ZybachAPIControllersWellChartDataDto { 
    TimeSeries?: Array<ZybachModelsDataTransferObjectsDailyPumpedVolume>;
    Sensors?: Array<ZybachModelsDataTransferObjectsSensorSummaryDto>;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}