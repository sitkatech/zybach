import mongoose, { Schema, Document } from "mongoose";
import { RoleDBOptions } from "./role";

export interface UserInterface extends Document {
  UserGuid: string;
  FirstName: string;
  LastName: string;
  Email: string;
  Phone: string;
  CreateDate: Date;
  UpdateDate: Date;
  LastActivityDate: Date;
  DisclaimerAcknowledgedDate:Date;
  ReceiveSupportEmails: boolean;
  LoginName: string;
  Company: string;
  Role: string;
}

const UserSchema: Schema = new Schema({
    UserGuid: {type:String, required:true},
    FirstName: {type: String, required: true},
    LastName: {type: String, required: true},
    Email: {type: String, required: true},
    Phone: {type: String, required: false},
    CreateDate: {type: Date, required: true},
    UpdateDate: {type: Date, required: false},
    LastActivityDate: {type: Date, required: false},
    DisclaimerAcknowledgedDate: {type: Date, required: false},
    ReceiveSupportEmails: {type: Boolean, required: true},
    LoginName: {type: String, required: false},
    Company: {type: String, required: false},
    Role: {type: String, required: true, enum: RoleDBOptions}
});

const User = mongoose.model<UserInterface>("User", UserSchema, "User");
export default User;
