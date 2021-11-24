import { ChemigationPermitAnnualRecordUpsertDto } from "./chemigation-permit-annual-record-upsert-dto"

export class ChemigationPermitNewDto {
	ChemigationPermitNumber : number
	ChemigationPermitStatusID : number
    TownshipRangeSection : string
    ChemigationCountyID: number
    TotalAcresTreated: number
    ChemigationPermitAnnualRecord: ChemigationPermitAnnualRecordUpsertDto
    
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}