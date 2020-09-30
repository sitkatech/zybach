var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

const apiRouter = require('./app_api/routes/index');
const checkApiKey = function(req, res, next) {
  let key = req.get(process.env.API_KEY_NAME);
  if (key == null || key == undefined || key !== process.env.API_KEY_VALUE) {
    return res.status(401).json({status: 'error', reason: 'unauthenticated'});
  }
  next();
};

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.all('/api/*', checkApiKey);
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
