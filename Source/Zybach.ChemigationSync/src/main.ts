const dockerSecrets = require('@cloudreach/docker-secrets');
import { getSites, getWorkOrder, getWorkOrderSamples, createWorkOrder, createSite, createSample, deleteSample } from './geooptix-service';
import { MongoClient } from 'mongodb';
const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'
import { GeoOptixSite, InspectionManifest, Sample, WorkOrder } from './models';
import { getTwoDigitMonth } from './util';

appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();

let existingSites: GeoOptixSite[];

let workOrdersToCreate: WorkOrder[] = [];
let sitesToCreate: GeoOptixSite[] = [];
let samplesToCreate: Sample[] = [];
let samplesToDelete: Sample[] = [];

const main = async () => {
    existingSites = await getSites();

    // step 0. get inspection manifests from mongo
    const inspectionManifest = await getInspectionManifest();

    
    // step 1. process inspection manifests
    await processInspectionManifest(inspectionManifest);

    // step 2. make geooptix calls to sync
    syncChemigationInspections();
}

const getInspectionManifest = async () => {
    let srv = ""
    let authSource = `authSource=${config.DATABASE_NAME}`;
    
    if (!process.env["DEBUG"]) {
        srv = "+srv";
        authSource = ""
    }

    const connstring = `mongodb${srv}://${config.DATABASE_USER}:${config.DATABASE_PASSWORD}@${config.DATABASE_URI}/?${authSource}`;
    const client = await MongoClient.connect(connstring);

    const db = client.db(config.DATABASE_NAME);
    const collection = db.collection("InspectionManifest");

    const inspectionManifest: InspectionManifest = await collection.findOne({});

    await client.close();

    return inspectionManifest;
}

const processInspectionManifest = async (inspectionManifest: InspectionManifest) => {
    // presumably there should be some logic here about the lastChangedDate to determine if this manifest needs to be processed or not.

    for (const fieldAssignment of inspectionManifest.FieldAssignments) {
        let workOrder: WorkOrder = await getWorkOrder(fieldAssignment.CanonicalName);

        let samples: Sample[] = [];

        if (!workOrder) {
            workOrder = {
                Name: fieldAssignment.Name,
                CanonicalName: fieldAssignment.CanonicalName,
                StartDate: fieldAssignment.StartDate,
                FinishDate: fieldAssignment.FinishDate,
                Description: "Created by the Groundwater Management Program",
                TeamMembers: []
            }
            workOrdersToCreate.push(workOrder)
        } else {
            samples = await getWorkOrderSamples(fieldAssignment.CanonicalName);
        }

        // create samples
        for (const site of fieldAssignment.Sites) {
            // if the site doesn't already exist, and !insertMissingSites, skip it
            const existingSite = existingSites.find(x => x.CanonicalName === site.CanonicalName)


            if (!existingSite && !inspectionManifest.InsertMissingWells) {
                continue;
            }

            // if the site doesn't already exist and insertMissingSites === true, create it
            if (!existingSite && !sitesToCreate.some(x => x.CanonicalName === site.CanonicalName)) {

                sitesToCreate.push({
                    CanonicalName: site.CanonicalName,
                    Name: site.CanonicalName,
                    Description: "Created by the Groundwater Management Program",
                    Tags: site.Tags,
                    Properties: site.Properties ?? {},
                    Location: {
                        type: "Feature",
                        properties: {},
                        geometry: {
                            type: "Point",
                            coordinates: [site.Latitude, site.Longitude]
                        }
                    }
                })
            }

            // if there's no sample for that site and the specified protocol, create it.
            const existingSample = samples.find(x => x.WorkOrderCanonicalName === fieldAssignment.CanonicalName &&
                x.ProtocolCanonicalName === fieldAssignment.Protocol.CanonicalName &&
                x.ProtocolVersionNumber === fieldAssignment.Protocol.Version &&
                x.SiteCanonicalName === site.CanonicalName);

            if (existingSample) {
                continue;
            }

            const sampleName = `${fieldAssignment.CanonicalName}-${site.CanonicalName}-${fieldAssignment.StartDate.getFullYear()}${getTwoDigitMonth(fieldAssignment.StartDate)}`;

            samplesToCreate.push({
                SiteCanonicalName: site.CanonicalName,
                CanonicalName: sampleName,
                Name: sampleName,
                ProtocolVersionNumber: fieldAssignment.Protocol.Version,
                ProtocolCanonicalName: fieldAssignment.Protocol.CanonicalName,
                WorkOrderCanonicalName: fieldAssignment.CanonicalName,
                SampleDate: new Date(),
                MethodUpdateDate: new Date(),
                Tags: []
            });
        }

        if (inspectionManifest.DeleteOrphanedSamples) {
            samplesToDelete.push(...samples.filter(x => !fieldAssignment.Sites.some(site => site.CanonicalName === x.SiteCanonicalName)));
        }
    }
}

const syncChemigationInspections = async () => {
    for (const workOrder of workOrdersToCreate) {
        await createWorkOrder(workOrder);
    }

    for (const site of sitesToCreate) {
        await createSite(site);
    }

    for (const sample of samplesToCreate) {
        await createSample(sample);
    }

    for (const sample of samplesToDelete) {
        await deleteSample(sample);
    }
}

main();