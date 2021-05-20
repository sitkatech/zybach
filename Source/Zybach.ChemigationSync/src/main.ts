const dockerSecrets = require('@cloudreach/docker-secrets');
import { MongoClient } from 'mongodb';
import {getSites, getWorkOrder, getWorkOrderSamples} from './geooptix-service';

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'
appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();


const main = async () => {
    // step 0. get inspection manifests from mongo

    // step 1. process inspection manifests
}



main();