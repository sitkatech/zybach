const fs = require('fs');

const dockerSecret = {};

dockerSecret.read = function read(secretNameAndPath) {
  try {
    return fs.readFileSync(`/run/secrets/${secretNameAndPath}`, 'utf8');
  } catch(err) {
    //TODO implement logging here to ensure we know why we failed
    // if (err.code !== 'ENOENT') {
    //   log.error(`An error occurred while trying to read the secret: ${secretName}. Err: ${err}`);
    // } else {
    //   log.debug(`Could not find the secret, probably not running in swarm mode: ${secretName}. Err: ${err}`);
    // }    
    return false;
  }
};

module.exports = dockerSecret;