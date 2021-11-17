export class ChemigationPermitAnnualRecordUpsertDto {
	ChemigationPermitID : number
	ChemigationPermitAnnualRecordStatusID : number
    ApplicantFirstName : string
    ApplicantLastName : string
    PivotName : string
    RecordYear : number
	DateReceived : Date
    DatePaid : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

// update to look like this:
// ChemigationPermitAnnualRecordID : number
// ChemigationPermitID : number
// RecordYear : number
// ChemigationPermitAnnualRecordStatusID : number
// PivotName : string
// ChemigationInjectionUnitTypeID : number
// ApplicantFirstName : string
// ApplicantLastName : string
// ApplicantMailingAddress : string
// ApplicantCity : string
// ApplicantState : string
// ApplicantZipCode : number
// ApplicantPhone : string
// ApplicantMobilePhone : string
// DateReceived : Date
// DatePaid : Date