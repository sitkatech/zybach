//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWell]
import { WellDto } from './well-dto'

export class AgHubWellDto {
	AgHubWellID : number
	Well : WellDto
	WellTPID : string
	WellTPNRDPumpRate : number
	TPNRDPumpRateUpdated : Date
	WellConnectedMeter : boolean
	WellAuditPumpRate : number
	AuditPumpRateUpdated : Date
	HasElectricalData : boolean
	FetchDate : Date
	RegisteredPumpRate : number
	RegisteredUpdated : Date
	AgHubRegisteredUser : string
	FieldName : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
