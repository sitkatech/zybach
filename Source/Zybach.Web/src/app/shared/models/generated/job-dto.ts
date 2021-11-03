//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Job]


export class JobDto {
	Id : number
	StateId : number
	StateName : string
	InvocationData : string
	Arguments : string
	CreatedAt : Date
	ExpireAt : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
