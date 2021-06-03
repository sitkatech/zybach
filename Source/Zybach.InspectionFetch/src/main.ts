const dockerSecrets = require('@cloudreach/docker-secrets');
import { getSamples } from './geooptix-service';
import { MongoClient } from 'mongodb';
const config = JSON.parse(dockerSecrets.Inspection_Fetch_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'

const protocolCanonicalNames = ['water-quality-inspection', 'chemigation-inspection', 'nitrates-inspection', 'water-level-inspection' ]

appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();

const main = async () => {
    const samples = await getSamples();
    
    const inspections = samples.filter(x=> protocolCanonicalNames.includes(x.Protocol.CanonicalName))

    cacheInspections(inspections);

}

const cacheInspections = async (samples) => {
    console.log(samples);
}


main();
