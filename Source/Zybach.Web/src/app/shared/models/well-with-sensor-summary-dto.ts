export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    sensors: SensorSummaryDto[]
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}