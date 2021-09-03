import mongoose, { Schema, Document } from "mongoose";

export interface ChemigationInspectionInterface extends Document {
    wellRegistrationID: string
    protocolCanonicalName: string
    status: string
    lastUpdate: Date
}

const ChemigationInspectionSchema: Schema = new Schema({
    wellRegistrationID: { type: String, required: true },
    protocolCanonicalName: { type: String, required: true },
    status: { type: String, required: true },
    lastUpdate: { type: Date, required: true },
});

const ChemigationInspection = mongoose.model<ChemigationInspectionInterface>("ChemigationInspection", ChemigationInspectionSchema, "ChemigationInspection");
export default ChemigationInspection;
