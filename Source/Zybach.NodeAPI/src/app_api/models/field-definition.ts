import mongoose, { Schema, Document } from "mongoose";
import { RoleDto, UserDto } from "../dtos/user-dto";

export interface FieldDefinitionInterface extends Document {
  FieldDefinitionID: number,
  FieldDefinitionName: string,
  FieldDefinitionDisplayName: string,
  FieldDefinitionValue:string
}

const FieldDefinitionSchema: Schema = new Schema({
  FieldDefinitionID:{type: Number, required: true},
  FieldDefinitionName: {type: String, required: true},
  FieldDefinitionDisplayName: {type: String, required: true},
  FieldDefinitionValue: {type: String, required: true}
});

const FieldDefinition = mongoose.model<FieldDefinitionInterface>("FieldDefinition", FieldDefinitionSchema, "fieldDefinitions");
export default FieldDefinition;
