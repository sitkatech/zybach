//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]
import { ChemigationPermitDto } from './chemigation-permit-dto'
import { ChemigationPermitAnnualRecordStatusDto } from './chemigation-permit-annual-record-status-dto'
import { ChemigationInjectionUnitTypeDto } from './chemigation-injection-unit-type-dto'

export class ChemigationPermitAnnualRecordDto {
	ChemigationPermitAnnualRecordID : number
	ChemigationPermit : ChemigationPermitDto
	RecordYear : number
	ChemigationPermitAnnualRecordStatus : ChemigationPermitAnnualRecordStatusDto
	PivotName : string
	ChemigationInjectionUnitType : ChemigationInjectionUnitTypeDto
	ApplicantFirstName : string
	ApplicantLastName : string
	ApplicantMailingAddress : string
	ApplicantCity : string
	ApplicantState : string
	ApplicantZipCode : string
	ApplicantPhone : string
	ApplicantMobilePhone : string
	DateReceived : Date
	DatePaid : Date
	ApplicantEmail : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
