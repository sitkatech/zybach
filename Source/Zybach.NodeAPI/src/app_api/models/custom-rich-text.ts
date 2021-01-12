import mongoose, { Schema, Document } from "mongoose";
import { RoleDto, UserDto } from "../dtos/user-dto";

export interface CustomRichTextInterface extends Document {
    CustomRichTextID: number,
    CustomRichTextName: string,
    CustomRichTextDisplayName: string,
    CustomRichTextContent: string
}

const CustomRichTextSchema: Schema = new Schema({
    CustomRichTextID: { type: Number, required: true },
    CustomRichTextName: { type: String, required: true },
    CustomRichTextDisplayName: { type: String, required: true },
    CustomRichTextContent: { type: String, required: true }
});

const CustomRichText = mongoose.model<CustomRichTextInterface>("CustomRichText", CustomRichTextSchema, "CustomRichText");
export default CustomRichText;
