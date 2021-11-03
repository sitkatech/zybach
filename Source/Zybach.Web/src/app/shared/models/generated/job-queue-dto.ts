//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[JobQueue]


export class JobQueueDto {
	Id : number
	JobId : number
	Queue : string
	FetchedAt : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
