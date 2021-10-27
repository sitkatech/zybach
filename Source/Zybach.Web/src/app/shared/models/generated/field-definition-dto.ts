//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FieldDefinition]
import { FieldDefinitionTypeDto } from './field-definition-type-dto'

export class FieldDefinitionDto {
	FieldDefinitionID : number
	FieldDefinitionType : FieldDefinitionTypeDto
	FieldDefinitionValue : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}
