//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWell]


export class AgHubWellDto {
	AgHubWellID : number
	WellRegistrationID : string
	WellTPID : string
	TPNRDPumpRate : number
	TPNRDPumpRateUpdated : Date
	WellConnectedMeter : boolean
	WellAuditPumpRate : number
	AuditPumpRateUpdated : Date
	HasElectricalData : boolean
	FetchDate : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}