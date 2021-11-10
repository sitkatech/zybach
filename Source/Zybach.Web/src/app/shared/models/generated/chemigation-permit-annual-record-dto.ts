//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]
import { ChemigationPermitDto } from './chemigation-permit-dto'
import { ChemigationPermitAnnualRecordStatusDto } from './chemigation-permit-annual-record-status-dto'

export class ChemigationPermitAnnualRecordDto {
	ChemigationPermitAnnualRecordID : number
	ChemigationPermit : ChemigationPermitDto
	ChemigationPermitAnnualRecordStatus : ChemigationPermitAnnualRecordStatusDto
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
