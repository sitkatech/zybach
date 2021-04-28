import mongoose, { Schema, Document } from "mongoose";

export interface IrrigatedAcresPerYearInterface extends Document {
    year: number,
    acres: number
}

const IrrigatedAcresPerYearSchema: Schema = new Schema({
    year: {type:Number, required: true},
    acres: {type:Number, required: false}
})

export default IrrigatedAcresPerYearSchema;