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
import { ChemigationPermitAnnualRecordApplicatorUpsertDto } from '././chemigation-permit-annual-record-applicator-upsert-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationUpsertDto } from '././chemigation-permit-annual-record-chemical-formulation-upsert-dto';

export class ChemigationPermitAnnualRecordUpsertDto { 
    ChemigationPermitAnnualRecordStatusID: number;
    ChemigationPermitAnnualRecordFeeTypeID: number;
    ApplicantFirstName?: string;
    ApplicantLastName?: string;
    ApplicantCompany?: string;
    PivotName?: string;
    RecordYear: number;
    TownshipRangeSection: string;
    DateReceived?: string;
    DatePaid?: string;
    DateApproved?: string;
    ChemigationInjectionUnitTypeID: number;
    ApplicantPhone?: string;
    ApplicantMobilePhone?: string;
    ApplicantMailingAddress: string;
    ApplicantEmail?: string;
    ApplicantCity: string;
    ApplicantState: string;
    ApplicantZipCode: string;
    NDEEAmount?: number;
    AnnualNotes?: string;
    ChemicalFormulations?: Array<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto>;
    Applicators?: Array<ChemigationPermitAnnualRecordApplicatorUpsertDto>;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}