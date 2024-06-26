/**
 * Zybach.API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: 1.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { WellSimpleDto } from '././well-simple-dto';
import { WellGroupWellSimpleDto } from '././well-group-well-simple-dto';

export class WellGroupDto { 
    WellGroupID?: number;
    WellGroupName?: string;
    PrimaryWell?: WellSimpleDto;
    WellGroupWells?: Array<WellGroupWellSimpleDto>;
    HasWaterLevelInspections?: boolean;
    LatestWaterLevelInspectionDate?: string;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
