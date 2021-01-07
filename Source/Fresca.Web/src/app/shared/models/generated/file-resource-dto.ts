//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FileResource]
import { FileResourceMimeTypeDto } from './file-resource-mime-type-dto'
import { UserDto } from './user-dto'

export class FileResourceDto {
	FileResourceID : number
	FileResourceMimeType : FileResourceMimeTypeDto
	OriginalBaseFilename : string
	OriginalFileExtension : string
	FileResourceGUID : string
	FileResourceData : any
	CreateUser : UserDto
	CreateDate : Date

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}