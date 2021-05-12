import { AghubWellInterface } from "../models/aghub-well";
import { IrrigatedAcresPerYearInterface } from "../models/irrigated-acres-per-year";
import { AnnualPumpedVolumeDto } from "./annual-pumped-volume-dto";
import { IrrigatedAcresPerYearDto, IrrigatedAcresPerYearDtoFactory } from "./irrigated-acres-per-year-dto";

export interface WellSummaryDto {
    wellRegistrationID: string;
    wellTPID?: string;
    description?: string;
    location: any;
    lastReadingDate?: Date;
    firstReadingDate?: Date;
    inGeoOptix?: boolean;
    fetchDate?: Date;
    hasElectricalData?: boolean;
    irrigatedAcresPerYear?: IrrigatedAcresPerYearDto[];
}

export interface SensorSummaryDto {
    wellRegistrationID: string;
    sensorName?: string;
    sensorType: string;
}

export const SensorTypeMap: {[key: string]: string} = {
    "FlowMeter": "Flow Meter",
    "PumpMonitor": "Continuity Meter"
}

export interface WellWithSensorSummaryDto extends WellSummaryDto {
    sensors: SensorSummaryDto[]
}

export interface WellDetailDto extends WellWithSensorSummaryDto{
    annualPumpedVolume: AnnualPumpedVolumeDto[];
    inGeoOptix: boolean;
}

export class WellSummaryDtoFactory {
    public static fromAghubWell(model: AghubWellInterface) : WellWithSensorSummaryDto{
        const sensors: SensorSummaryDto[] = [];
        if (model.hasElectricalData){
            sensors.push({
                wellRegistrationID: model.wellRegistrationID,
                sensorType: "Electrical Usage"
            })
        }
        return {
            wellRegistrationID: model.wellRegistrationID,
            wellTPID: model.wellTPID,
            location: { geometry: model.location, type: "Feature", properties: {} },
            sensors: sensors,
            fetchDate: model.fetchDate,
            hasElectricalData: model.hasElectricalData,
            irrigatedAcresPerYear: model.irrigatedAcresPerYear.map(x => IrrigatedAcresPerYearDtoFactory.FromModel(x))
        }
    }
}