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
    InGeoOptix?: boolean;
}


export class SensorSummaryDto {
    WellRegistrationID: string;
    SensorName: string;
    SensorType: string;
}

export class SensorMessageAgeDto {
    WellRegistrationID: string;
    MessageAge: number;
    SensorType: string;
}