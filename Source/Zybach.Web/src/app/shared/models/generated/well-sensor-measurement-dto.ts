//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurement]
import { MeasurementTypeDto } from './measurement-type-dto'

export class WellSensorMeasurementDto {
	WellSensorMeasurementID : number
	WellRegistrationID : string
	MeasurementType : MeasurementTypeDto
	ReadingYear : number
	ReadingMonth : number
	ReadingDay : number
	SensorName : string
	MeasurementValue : number

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
