
export class ChemigationPermitUpsertDto {
	ChemigationPermitNumber : number
	ChemigationPermitStatusID : number
	TownshipRangeSection : string
	ChemigationCountyID : number
	
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}