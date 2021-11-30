//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcre]
import { AgHubWellDto } from './ag-hub-well-dto'

export class AgHubWellIrrigatedAcreDto {
	AgHubWellIrrigatedAcreID : number
	AgHubWell : AgHubWellDto
	IrrigationYear : number
	Acres : number

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
