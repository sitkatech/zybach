export interface WellSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
}

export interface SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}

export interface WellWithSensorSummaryDto extends WellSummaryDto {
    sensors: SensorSummaryDto[]
}