export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
    sensors: SensorSummaryDto[]
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}