//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurementStaging]
import { MeasurementTypeDto } from './measurement-type-dto'

export class WellSensorMeasurementStagingDto {
	WellSensorMeasurementStagingID : number
	WellRegistrationID : string
	MeasurementType : MeasurementTypeDto
	ReadingDate : Date
	SensorName : string
	MeasurementValue : number

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
