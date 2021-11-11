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