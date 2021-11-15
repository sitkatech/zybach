//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermit]
import { ChemigationPermitStatusDto } from './chemigation-permit-status-dto'
import { ChemigationCountyDto } from './chemigation-county-dto'

export class ChemigationPermitDto {
	ChemigationPermitID : number
	ChemigationPermitNumber : number
	ChemigationPermitStatus : ChemigationPermitStatusDto
	DateCreated : Date
	TownshipRangeSection : string
	ChemigationCounty : ChemigationCountyDto

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
