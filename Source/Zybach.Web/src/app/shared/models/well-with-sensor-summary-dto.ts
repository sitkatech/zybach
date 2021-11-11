import { IrrigatedAcresPerYearDto } from "./irrigated-acres-per-year-dto";

export class WellWithSensorSummaryDto {
    WellRegistrationID: string;
    Description: string;
    Location: any;
    LastReadingDate?: Date;
    FirstReadingDate?: Date;
    WellTPID?: string;
    Sensors: SensorSummaryDto[];
    HasElectricalData: any;
    IrrigatedAcresPerYear: IrrigatedAcresPerYearDto[];
    AgHubRegisteredUser: string;
    FieldName: string;
    InAgHub: boolean;
    InGeoOptix: boolean;
    IsActive: boolean;
}

export class WellWithSensorMessageAgeDto {
    WellRegistrationID: string;
    Location: any;
    Sensors: SensorMessageAgeDto[];
    AgHubRegisteredUser: string;
    FieldName: string;
}

export class WellDetailDto extends WellWithSensorSummaryDto {
    AnnualPumpedVolume?: any[];
}


export class SensorSummaryDto {
    WellRegistrationID: string;
    SensorName: string;
    SensorType: string;
    IsActive: boolean;
}

export class SensorMessageAgeDto {
    WellRegistrationID: string;
    MessageAge: number;
    SensorType: string;
    IsActive: boolean;
}