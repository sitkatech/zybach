import secrets from './secrets';
delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights';
if (process.env["ENVIRONMENT"] !== "DEBUG"){
  appInsights.setup(secrets.APPINSIGHTS_INSTRUMENTATIONKEY)
  .setAutoCollectConsole(true, true)
  .start();
}

import createError from 'http-errors';
import express, { NextFunction, Request, Response } from 'express';
import path from 'path';
import cookieParser from 'cookie-parser';
import bodyParser from "body-parser";
import logger from 'morgan';
import { ApiError } from 'errors/apiError';
import { ValidateError } from 'tsoa';

import jwt from 'express-jwt';
import jwksRsa from 'jwks-rsa';

import {RegisterRoutes} from './routes/generated/routes'
import connect from "./connect";
import cors from 'cors';

import swaggerUi from "swagger-ui-express";
import swaggerDoc from "./swagger.json";

connect();
const app = express();

app.use(cors({
  origin: "*",
  optionsSuccessStatus: 200
}));

app.use(jwt({
  secret: jwksRsa.expressJwtSecret({
    cache:true,
    rateLimit: true,
    jwksRequestsPerMinute: 5,
    jwksUri: `${process.env["KEYSTONE_BASE_URL"]}/.well-known/openid-configuration/jwks`,
  }),
  algorithms: ['RS256'],
  requestProperty: "auth",
  credentialsRequired: false,
  
}));

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(bodyParser.json());

RegisterRoutes(app);

app.use("/api/docs", swaggerUi.serve, swaggerUi.setup(swaggerDoc, {
  swaggerOptions:{
    defaultModelsExpandDepth: -1
  }
}));

app.use(function (req, res, next) {
  next(createError(404));
});


// error handler
app.use(function (err: ApiError, req: Request, res: Response, next: NextFunction) {
  // set locals, only providing error in development
  if (err instanceof ValidateError) {
    console.error(`Caught Validation Error for ${req.path}:`, err.fields);
    return res.status(422).json({
      message: "Validation Failed",
      details: err?.fields,
    });
  }
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  console.error(err);

  // render the error page
  res.status(err.status || 500).json({ status: err.status, message: err.message });
});

export default app;