import mongoose, { Schema, Document } from "mongoose";
import IrrigatedAcresPerYearSchema, { IrrigatedAcresPerYearInterface } from "./irrigated-acres-per-year";

export interface AghubWellInterface extends Document {
    wellRegistrationID: string;
    wellTPID: string;
    wellConnectedMeter: 0 | 1;
    wellAuditPumpRate: number
    auditPumpRateUpdated: string;
    wellTpnrdPumpRate: number;
    tpnrdPumpRateUpdated: string;
    location: any;
    fetchDate: Date;
    hasElectricalData: boolean;
    irrigatedAcresPerYear: IrrigatedAcresPerYearInterface[];
}

const AghubWellSchema: Schema = new Schema({
    wellRegistrationID: {type:String, required: true},
    wellTPID:{type:String, required: true},
    wellConnectedMeter: {type:Number, required: true, enum: [0,1]},
    wellAuditPumpRate: {type:Number, required: true},
    auditPumpRateUpdated: {type:String, required: true},
    wellTpnrdPumpRate: {type:Number, required: true},
    tpnrdPumpRateUpdated: {type:String, required: true},
    location: {type: Object, required: true},
    fetchDate: {type: Date, required: true},
    hasElectricalData: {type: Boolean, required: true},
    irrigatedAcresPerYear: {type: [IrrigatedAcresPerYearSchema], required:false}
})
const AghubWell = mongoose.model<AghubWellInterface>("agHubWells", AghubWellSchema, "agHubWells");
export default AghubWell;