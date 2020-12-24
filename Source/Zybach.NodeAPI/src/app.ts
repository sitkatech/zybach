import secrets from './secrets';
delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights';
appInsights.setup(secrets.APPINSIGHTS_INSTRUMENTATIONKEY)
  .setAutoCollectConsole(true, true)
  .start();

  //const createError = require('http-errors');

import createError from 'http-errors';

import express, { NextFunction, Request, Response } from 'express';
import path from 'path';
import cookieParser from 'cookie-parser';
import logger from 'morgan';
import swaggerUi from 'swagger-ui-express'
//const specs = require('../../swagger');
import apiRouter from './app_api/routes/index'

const app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/api', apiRouter);
// app.get('/api-docs/swagger.json', function(req, res) {
//   res.setHeader('Content-Type', 'application/json');
//   res.send(JSON.stringify(specs, null, 4));
// });
// app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(specs));

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err: { message: any; status: any; }, req: Request, res: Response, next: NextFunction) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  console.error(err);

  // render the error page
  res.status(err.status || 500).json({ status: err.message });
});

export default app;
