export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
    firstReadingDate?: Date;
    wellTPID?: string;
    sensors: SensorSummaryDto[]
    annualPumpedVolume?: any[];
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}