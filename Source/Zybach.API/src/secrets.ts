const dockerSecrets = require('@cloudreach/docker-secrets');

export default JSON.parse(dockerSecrets.NODE_API_SECRETS);