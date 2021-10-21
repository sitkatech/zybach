//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Sensor]
import { SensorTypeDto } from './sensor-type-dto'
import { WellDto } from './well-dto'

export class SensorDto {
	SensorID : number
	SensorName : string
	SensorType : SensorTypeDto
	Well : WellDto
	InGeoOptix : boolean
	CreateDate : Date
	LastUpdateDate : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
