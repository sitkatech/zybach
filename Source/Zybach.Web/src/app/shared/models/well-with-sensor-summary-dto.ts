export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
    wellTPID?: string;
    sensors: SensorSummaryDto[]
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}