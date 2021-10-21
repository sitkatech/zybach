//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellIrrigatedAcre]
import { WellDto } from './well-dto'

export class WellIrrigatedAcreDto {
	WellIrrigatedAcreID : number
	Well : WellDto
	IrrigationYear : number
	Acres : number

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
