const dockerSecrets = require('@cloudreach/docker-secrets');
import { MongoClient } from 'mongodb';
import {getSites, getWorkOrder, getWorkOrderSamples} from './geooptix-service';

const config = JSON.parse(dockerSecrets.Chemigation_Sync_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'
import inspectionManifestMock from 'data/inspectionManifest';
import { FieldAssignment, GeoOptixSite, InspectionManifest, Sample, WorkOrder } from 'models';

appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();

let existingSites: GeoOptixSite[];

let workOrdersToCreate: WorkOrder[] = [];
let sitesToCreate: GeoOptixSite[] = [];
let samplesToCreate: Sample[] = [];
let samplesToDeleteCnames = []

const main = async () => {
    existingSites = await getSites();

    // step 0. get inspection manifests from mongo
    const inspectionManifest = getInspectionManifest();


    // step 1. process inspection manifests
    processInspectionManifest(inspectionManifest);
}

const getInspectionManifest = () =>{
    // todo
    return inspectionManifestMock
}

const processInspectionManifest = async (inspectionManifest: InspectionManifest) => {
    // presumably there should be some logic here about the lastChangedDate to determine if this manifest needs to be processed or not.

    for (const fieldAssignment of inspectionManifest.fieldAssignments){
        let workOrder: WorkOrder = await getWorkOrder(fieldAssignment.cname);

        let samples: Sample[] = [];

        if (!workOrder) {
            workOrder = {
                Name: fieldAssignment.name,
                CanonicalName: fieldAssignment.cname,
                StartDate: fieldAssignment.startDate,
                FinishDate: fieldAssignment.endDate,
                Description: "Created by the Groundwater Management Program",
                TeamMembers: []
            }
            workOrdersToCreate.push()
        } else {
            samples = await getWorkOrderSamples(fieldAssignment.cname);
        }

        for (const site of fieldAssignment.sites){
            // if the site doesn't already exist, and !insertMissingSites, skip it
            const existingSite = existingSites.find(x=>x.CanonicalName === site.cname)


            if (!existingSite && !inspectionManifest.insertMissingWells){
                continue;
            }

            // if the site doesn't already exist and insertMissingSites === true, create it
            if (!existingSite && !sitesToCreate.some(x=>x.CanonicalName === site.cname)){
                
                sitesToCreate.push({
                    CanonicalName: site.cname,
                    Name: site.cname,
                    Description: "Created by the Groundwater Management Program",
                    Tags: site.tags,
                    Properties: site.properties,
                    Location: {
                        type: "Feature",
                        properties: {},
                        geometry: {
                            type: "Point",
                            coordinates: [site.longitude, site.latitude]
                        }
                    }
                })
            }

            // if there's no sample for that site and the specified protocol, create it.
            const existingSample = samples.find(x=>x.WorkOrderCanonicalName === fieldAssignment.cname &&
                x.ProtocolCanonicalName === fieldAssignment.protocol.cname &&
                x.ProtocolVersionNumber === fieldAssignment.protocol.version);
            
            if (existingSample){
                continue;
            }

            // samplesToCreate.push({
            //     SiteCanonicalName: site.cname,
            //     Name: 
            // })
        }



    }
}

main();