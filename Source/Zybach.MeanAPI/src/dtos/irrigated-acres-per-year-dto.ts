import { IrrigatedAcresPerYearInterface } from "../models/irrigated-acres-per-year";

export class IrrigatedAcresPerYearDto {
    Year?: number;
    Acres?: number;

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

export class IrrigatedAcresPerYearDtoFactory {
    public static FromModel (model: IrrigatedAcresPerYearInterface): IrrigatedAcresPerYearDto {
        return new IrrigatedAcresPerYearDto({
            Year: model.year,
            Acres: model.acres
        })
    }
}
