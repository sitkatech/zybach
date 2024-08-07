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
import { ChemigationPermitDto } from '././chemigation-permit-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationSimpleDto } from '././chemigation-permit-annual-record-chemical-formulation-simple-dto';
import { ChemigationInspectionSimpleDto } from '././chemigation-inspection-simple-dto';
import { ChemigationPermitAnnualRecordApplicatorSimpleDto } from '././chemigation-permit-annual-record-applicator-simple-dto';

export class ChemigationPermitAnnualRecordDetailedDto { 
    ChemigationPermitAnnualRecordID?: number;
    ChemigationPermit?: ChemigationPermitDto;
    RecordYear?: number;
    TownshipRangeSection?: string;
    ChemigationPermitAnnualRecordStatusID?: number;
    ChemigationPermitAnnualRecordStatusName?: string;
    PivotName?: string;
    ChemigationInjectionUnitTypeID?: number;
    ChemigationInjectionUnitTypeName?: string;
    ChemigationPermitAnnualRecordFeeTypeID?: number;
    ChemigationPermitAnnualRecordFeeTypeName?: string;
    ApplicantFirstName?: string;
    ApplicantLastName?: string;
    ApplicantCompany?: string;
    ApplicantMailingAddress?: string;
    ApplicantCity?: string;
    ApplicantState?: string;
    ApplicantZipCode?: string;
    ApplicantPhone?: string;
    ApplicantMobilePhone?: string;
    DateReceived?: string;
    DatePaid?: string;
    DateApproved?: string;
    ApplicantEmail?: string;
    NDEEAmount?: number;
    AnnualNotes?: string;
    ApplicantName?: string;
    ChemicalFormulations?: Array<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>;
    Applicators?: Array<ChemigationPermitAnnualRecordApplicatorSimpleDto>;
    Inspections?: Array<ChemigationInspectionSimpleDto>;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
