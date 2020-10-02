var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
var secrets = require('./secrets');

const apiRouter = require('./app_api/routes/index');
const checkApiKey = function(req, res, next) {
  let keyValue = secrets.API_KEY_VALUE;
  let keySent = req.get('authorization');
  if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
    return res.status(401).json({status: 'error', reason: 'unauthorized'});
  }
  next();
};

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.all('/api/wells/*', checkApiKey);
app.use('/api', apiRouter);


// catch 404 and forward to error handler
app.use(function(req, res, next) {
  next(createError(404));
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
});

module.exports = app;
