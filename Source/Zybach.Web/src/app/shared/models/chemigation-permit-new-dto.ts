
export class ChemigationPermitNewDto {
	ChemigationPermitNumber : number
	ChemigationPermitStatusID : number
    TownshipRangeSection : string
    ChemigationCountyID: number
    TotalAcresTreated: number
    ChemigationInjectionUnitTypeID: number
    ApplicantFirstName : string
    ApplicantLastName : string
    ApplicantPhone : string
    ApplicantMobilePhone : string
    ApplicantEmail: string
    ApplicantMailingAddress : string
    ApplicantCity : string
    ApplicantState : string
    ApplicantZipCode : number
    PivotName : string
    
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}