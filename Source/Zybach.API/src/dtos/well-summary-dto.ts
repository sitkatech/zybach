import { AghubWellInterface } from "../models/aghub-well";

export interface WellSummaryDto {
    wellRegistrationID: string;
    wellTPID?: string;
    description?: string;
    location: any;
    lastReadingDate?: Date;
}

export interface SensorSummaryDto {
    wellRegistrationID: string;
    sensorName?: string;
    sensorType: string;
}

export interface WellWithSensorSummaryDto extends WellSummaryDto {
    sensors: SensorSummaryDto[]
}

export class WellSummaryDtoFactory {
    public static fromAghubWell(model: AghubWellInterface) : WellWithSensorSummaryDto{
        const sensors: SensorSummaryDto[] = [];
        if (model.wellConnectedMeter){
            sensors.push({
                wellRegistrationID: model.wellRegistrationID,
                sensorType: "Electrical Data"
            })
        }
        return {
            wellRegistrationID: model.wellRegistrationID,
            wellTPID: model.wellTPID,
            location: { geometry: model.location, type: "Feature", properties: {} },
            sensors: sensors
        }
    }
}