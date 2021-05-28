import { IrrigatedAcresPerYearDto } from "./irrigated-acres-per-year-dto";

export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
    firstReadingDate?: Date;
    wellTPID?: string;
    sensors: SensorSummaryDto[];
    hasElectricalData: any;
    irrigatedAcresPerYear: IrrigatedAcresPerYearDto[];
}

export class WellWithSensorMessageAgeDto {
    wellRegistrationID: string;
    location: any;
    sensors: SensorMessageAgeDto[];
}

export class WellDetailDto extends WellWithSensorSummaryDto {
    annualPumpedVolume?: any[];
    inGeoOptix?: boolean;
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}

export class SensorMessageAgeDto {
    wellRegistrationID: string;
    messageAge: number;
    sensorType: string;
}