export class IrrigatedAcresPerYearDto {
    Year?: number;
    Acres?: number;

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
