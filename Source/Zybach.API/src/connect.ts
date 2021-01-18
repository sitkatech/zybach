import mongoose from "mongoose";
import secrets from "./secrets";

const protocol = process.env["ENVIRONMENT"] === "DEBUG" ? "mongodb" : "mongodb+srv";

const connectionString = `${protocol}://${secrets.DATABASE_USER}:${secrets.DATABASE_PASSWORD}@${secrets.DATABASE_URI}`;
export default () => {
  const connect = () => {
    mongoose
      .connect(connectionString, { useNewUrlParser: true })
      .then(() => {
        return console.log(`Successfully connected to database`);
      })
      .catch(error => {
        console.log("Error connecting to database: ", error);
        return process.exit(1);
      });
  };
  connect();

  mongoose.connection.on("disconnected", connect);
};