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
import { ChemicalUnitDto } from '././chemical-unit-dto';

export class ChemicalFormulationYearlyTotalDto { 
    RecordYear?: number;
    ChemicalFormulation?: string;
    TotalApplied?: number;
    ChemicalUnit?: ChemicalUnitDto;
    AcresTreated?: number;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}