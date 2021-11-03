//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[State]
import { JobDto } from './job-dto'

export class StateDto {
	Id : number
	Job : JobDto
	Name : string
	Reason : string
	CreatedAt : Date
	Data : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
