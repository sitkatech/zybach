const secrets = require("@cloudreach/docker-secrets");

module.exports = JSON.parse(secrets.TEASPOON_INFLUX_SECRETS)
