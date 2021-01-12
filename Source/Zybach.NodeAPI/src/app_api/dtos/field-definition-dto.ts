import FieldDefinition, { FieldDefinitionInterface } from "../models/field-definition";

export class FieldDefinitionDto {
	FieldDefinitionID? : number
	FieldDefinitionType? : FieldDefinitionTypeDto
	FieldDefinitionValue? : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

export class FieldDefinitionTypeDto {
	FieldDefinitionTypeID? : number
	FieldDefinitionTypeName? : string
	FieldDefinitionTypeDisplayName? : string

    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

export class FieldDefinitionDtoFactory {
    public static FromModel (model: FieldDefinitionInterface){
        const fieldDefinitionType = {
            FieldDefinitionTypeID: model.FieldDefinitionID,
            FieldDefinitionTypeName: model.FieldDefinitionName,
            FieldDefinitionTypeDisplayName: model.FieldDefinitionDisplayName
        }

        return new FieldDefinition({
            FieldDefinitionID: model.FieldDefinitionID,
            FieldDefinitionType: fieldDefinitionType,
            FieldDefinitionValue: model.FieldDefinitionValue
        });
    }
}