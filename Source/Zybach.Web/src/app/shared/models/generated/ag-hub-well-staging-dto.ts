//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellStaging]


export class AgHubWellStagingDto {
	AgHubWellStagingID : number
	WellRegistrationID : string
	WellTPID : string
	TPNRDPumpRate : number
	TPNRDPumpRateUpdated : Date
	WellConnectedMeter : boolean
	WellAuditPumpRate : number
	AuditPumpRateUpdated : Date
	HasElectricalData : boolean

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
