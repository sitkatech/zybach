
export class ChemigationPermitNewDto {
	ChemigationPermitNumber : number
	ChemigationPermitStatusID : number
    TownshipRangeSection : string
    ApplicantFirstName : string
    ApplicantLastName : string
    PivotName : string
	DateReceived : Date
    DatePaid : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}