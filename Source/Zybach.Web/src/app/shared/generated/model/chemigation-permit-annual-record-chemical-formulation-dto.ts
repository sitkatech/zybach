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
import { ChemicalFormulationDto } from '././chemical-formulation-dto';
import { ChemigationPermitAnnualRecordDto } from '././chemigation-permit-annual-record-dto';
import { ChemicalUnitDto } from '././chemical-unit-dto';

export class ChemigationPermitAnnualRecordChemicalFormulationDto { 
    ChemigationPermitAnnualRecordChemicalFormulationID?: number;
    ChemigationPermitAnnualRecord?: ChemigationPermitAnnualRecordDto;
    ChemicalFormulation?: ChemicalFormulationDto;
    ChemicalUnit?: ChemicalUnitDto;
    TotalApplied?: number;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}