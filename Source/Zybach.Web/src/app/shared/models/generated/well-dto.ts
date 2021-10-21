//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
import { StreamFlowZoneDto } from './stream-flow-zone-dto'

export class WellDto {
	WellID : number
	WellRegistrationID : string
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
	StreamflowZone : StreamFlowZoneDto
	LandownerName : string
	FieldName : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
