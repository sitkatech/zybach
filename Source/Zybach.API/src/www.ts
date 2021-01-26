#!/usr/bucket/env node

/**
 * Module dependencies.
 */

import app from './app';
import Debug from 'debug';
const debug = Debug('zybachapi:server');

import fs from 'fs';
import http from 'http';


import https from 'https';

/**
 * Get port from environment and store in Express.
 */

var port = normalizePort(process.env.PORT || '3000');
app.set('port', port);

/**
 * Create HTTP server.
 */

let server: http.Server | https.Server;

if (process.env["ENVIRONMENT"] === "DEBUG") {
  const key = fs.readFileSync('/src/key.pem');
  const cert = fs.readFileSync('/src/cert.pem');
  server = https.createServer({ key: key, cert: cert }, app);
} else {
  server = http.createServer(app);
}
/**
 * Listen on provided port, on all network interfaces.
 */

server.listen(port);
server.on('error', onError);
server.on('listening', onListening);

/**
 * Normalize a port into a number, string, or false.
 */

function normalizePort(val: string) {
  var port = parseInt(val, 10);

  if (isNaN(port)) {
    // named pipe
    return val;
  }

  if (port >= 0) {
    // port number
    return port;
  }

  return false;
}

/**
 * Event listener for HTTP server "error" event.
 */

function onError(error: NodeJS.ErrnoException) {
  if (error.syscall !== 'listen') {
    throw error;
  }

  var bind = typeof port === 'string'
    ? 'Pipe ' + port
    : 'Port ' + port;

  // handle specific listen errors with friendly messages
  switch (error.code) {
    case 'EACCES':
      console.error(bind + ' requires elevated privileges');
      process.exit(1);
      break;
    case 'EADDRINUSE':
      console.error(bind + ' is already in use');
      process.exit(1);
      break;
    default:
      throw error;
  }
}

/**
 * Event listener for HTTP server "listening" event.
 */

function onListening() {
  var addr = server.address();
  if (addr === null) {
    throw new Error("This shouldn't happen!!!!!!!!!!!");
  }
  var bind = typeof addr === 'string'
    ? 'pipe ' + addr
    : 'port ' + addr.port;
  debug('Listening on ' + bind);
}
