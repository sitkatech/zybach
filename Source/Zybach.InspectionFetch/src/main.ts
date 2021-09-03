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
    let srv = ""
    let authSource = `authSource=${config.DATABASE_NAME}`;
    
    if (!process.env["DEBUG"]) {
        srv = "+srv";
        authSource = ""
    }

    const connstring = `mongodb${srv}://${config.DATABASE_USER}:${config.DATABASE_PASSWORD}@${config.DATABASE_URI}/?${authSource}`;
    const client = await MongoClient.connect(connstring);

    const db = client.db(config.DATABASE_NAME);
    const collection = db.collection("ChemigationInspection");

    await collection.deleteMany({});
    await collection.insertMany(samples.map(x=> ({
        wellRegistrationID: x.Site.CanonicalName,
        protocolCanonicalName: x.Protocol.CanonicalName,
        status: x.Status,
        lastUpdate: new Date(x.UpdateDate || x.CreateDate)
    })));

    await client.close();
}


main();
