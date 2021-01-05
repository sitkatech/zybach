import mongoose, { Schema, Document } from "mongoose";

export interface GeoOptixTokenInterface extends Document {
  TokenValue: string;
  ExpirationDate: Date;
}

const GeoOptixTokenSchema: Schema = new Schema({
  TokenValue: { type: String, required: true },
  ExpirationDate: { type: Date, required: true }
});

const GeoOptixToken = mongoose.model<GeoOptixTokenInterface>("GeoOptixToken", GeoOptixTokenSchema);
export default GeoOptixToken;
