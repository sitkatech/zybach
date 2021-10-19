//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellStaging]


export class AgHubWellStagingDto {
	AgHubWellStagingID : number
	WellRegistrationID : string
	WellTPID : string
	WellTPNRDPumpRate : number
	TPNRDPumpRateUpdated : Date
	WellConnectedMeter : boolean
	WellAuditPumpRate : number
	AuditPumpRateUpdated : Date
	RegisteredPumpRate : number
	RegisteredUpdated : Date
	HasElectricalData : boolean
	LandownerName : string
	FieldName : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
