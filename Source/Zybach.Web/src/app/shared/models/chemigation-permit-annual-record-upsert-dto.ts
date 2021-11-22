import { ChemigationPermitAnnualRecordStatusEnum } from "./enums/chemigation-permit-annual-record-status.enum"
import { ChemigationPermitAnnualRecordDto } from "./generated/chemigation-permit-annual-record-dto"

export class ChemigationPermitAnnualRecordUpsertDto {
    ChemigationPermitID : number
	ChemigationPermitAnnualRecordStatusID : number
    ChemigationInjectionUnitTypeID : number
    PivotName : string
    RecordYear: number
    ApplicantFirstName : string
    ApplicantLastName : string
    ApplicantMailingAddress : string
    ApplicantCity : string
    ApplicantState : string
    ApplicantZipCode : number
    ApplicantPhone : string
    ApplicantMobilePhone : string
    ApplicantEmail : string
    DateReceived : Date
    DatePaid : Date

    // constructor(obj?: any) {
    //     Object.assign(this, obj);
    // }

    constructor(annualRecord: ChemigationPermitAnnualRecordDto, recordYear: number, chemigationPermitAnnualRecordStatusEnum: ChemigationPermitAnnualRecordStatusEnum) {
        this.ChemigationPermitID = annualRecord.ChemigationPermit.ChemigationPermitID;
        this.ChemigationPermitAnnualRecordStatusID = annualRecord.ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusID;
        this.ChemigationInjectionUnitTypeID = annualRecord.ChemigationInjectionUnitType.ChemigationInjectionUnitTypeID;
        this.RecordYear = annualRecord.RecordYear;
        this.PivotName = annualRecord.PivotName;
        this.ApplicantFirstName = annualRecord.ApplicantFirstName;
        this.ApplicantLastName = annualRecord.ApplicantLastName;
        this.ApplicantMailingAddress = annualRecord.ApplicantMailingAddress;
        this.ApplicantCity = annualRecord.ApplicantCity;
        this.ApplicantState = annualRecord.ApplicantState;
        this.ApplicantZipCode = annualRecord.ApplicantZipCode;
        this.ApplicantPhone = annualRecord.ApplicantPhone;
        this.ApplicantMobilePhone = annualRecord.ApplicantMobilePhone;
        this.ApplicantEmail = annualRecord.ApplicantEmail;
        this.DateReceived = annualRecord.DateReceived;
        this.DatePaid = annualRecord.DatePaid;

        this.RecordYear = recordYear;
        this.ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecordStatusEnum;
    }
}