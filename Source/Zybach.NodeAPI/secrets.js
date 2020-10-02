const secrets = require('@cloudreach/docker-secrets');

module.exports = JSON.parse(secrets.NODE_API_SECRETS);