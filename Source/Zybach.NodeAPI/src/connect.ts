import mongoose from "mongoose";
import secrets from "./secrets";

export default () => {
  const connect = () => {
    mongoose
      .connect(secrets.DATABASE_URI, { useNewUrlParser: true })
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