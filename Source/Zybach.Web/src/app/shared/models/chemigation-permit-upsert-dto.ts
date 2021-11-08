
export class ChemigationPermitUpsertDto {
	ChemigationPermitNumber : number
	ChemigationPermitStatusID : number
	DateReceived : Date
	TownshipRangeSection : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}