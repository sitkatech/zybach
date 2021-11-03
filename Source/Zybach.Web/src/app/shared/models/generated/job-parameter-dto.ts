//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[JobParameter]
import { JobDto } from './job-dto'

export class JobParameterDto {
	Job : JobDto
	Name : string
	Value : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
