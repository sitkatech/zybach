//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[User]
import { RoleDto } from './role-dto'

export class UserDto {
	UserID : number
	UserGuid? : string
	FirstName : string
	LastName : string
	Email : string
	Phone? : string
	Role : RoleDto
	CreateDate : Date
	UpdateDate? : Date
	LastActivityDate? : Date
	DisclaimerAcknowledgedDate? : Date
	IsActive : boolean
	ReceiveSupportEmails : boolean
	LoginName? : string
	Company? : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}