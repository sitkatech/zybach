export interface WellSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
}

export interface SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}

export interface WellWithSensorSummaryDto extends WellSummaryDto {
    sensors: SensorSummaryDto[]
}