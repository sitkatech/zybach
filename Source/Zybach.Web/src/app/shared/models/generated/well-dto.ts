//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]
import { StreamFlowZoneDto } from './stream-flow-zone-dto'

export class WellDto {
	WellID : number
	WellRegistrationID : string
	StreamflowZone : StreamFlowZoneDto
	CreateDate : Date
	LastUpdateDate : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
