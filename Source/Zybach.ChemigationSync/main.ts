const dockerSecrets = require('@cloudreach/docker-secrets');
import got from 'got';
import { MongoClient } from 'mongodb';

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'
appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();
