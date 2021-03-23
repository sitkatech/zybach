import { IrrigatedAcresPerYearDto } from "./irrigated-acres-per-year-dto";

export class WellWithSensorSummaryDto {
    wellRegistrationID: string;
    description: string;
    location: any;
    lastReadingDate?: Date;
    firstReadingDate?: Date;
    wellTPID?: string;
    sensors: SensorSummaryDto[];
    annualPumpedVolume?: any[];
    hasElectricalData: any;
    irrigatedAcresPerYear: IrrigatedAcresPerYearDto[];
}


export class SensorSummaryDto {
    wellRegistrationID: string;
    sensorName: string;
    sensorType: string;
}