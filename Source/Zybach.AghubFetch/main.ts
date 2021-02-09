const dockerSecrets = require('@cloudreach/docker-secrets');
import got from 'got';
import { MongoClient } from 'mongodb';
import { DateTime } from 'luxon';
import { InfluxDB } from "@influxdata/influxdb-client";

const config = JSON.parse(dockerSecrets.Aghub_Fetch_Secret);

delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
import * as appInsights from 'applicationinsights'
appInsights.setup(config.APPINSIGHTS_INSTRUMENTATIONKEY)
    .setAutoCollectConsole(true, true)
    .setAutoCollectExceptions(true)
    .start();

const influxClient = new InfluxDB({ url: config.INFLUX_DB_URL, token: config.INFLUX_DB_TOKEN });
const influxQueryApi = influxClient.getQueryApi(config.INFLUX_DB_ORG);

let lastReadingDates: {[key:string ]:Date} = {};

async function main() {
    const wells = await getWellCollection();

    lastReadingDates = await getLastReadingDatetime();

    await cacheWellsInDb(wells);

    for (let i = 0; i < wells.length; i++) {
        await processWell(wells[i]);
    }
}

async function getWellCollection(): Promise<agHubWell[]> {
    let result;
    try {
        const wellCollectionUrl = config.COLLECTION_URL;
        result = await got.get(wellCollectionUrl, {
            headers: {
                "x-api-key": config.COLLECTION_API_KEY
            },
            responseType: "json",
        });

    } catch (err) {
        console.error(err)
        throw err;
    }

    if (result.statusCode !== 200) {
        const message = `Could not get Well collection. ${result.statusCode}: ${result.body.message}`;
        console.error(message);
        throw new Error(message);
    }

    return result.body.data;
}

async function getLastReadingDatetime(): Promise<{ [key: string]: Date }> {
    const query = `from(bucket: "${config.INFLUX_DB_BUCKET}") \
    |> range(start: 2000-01-01T00:00:00Z) \
    |> filter(fn: (r) => r["_measurement"] == "estimated-pumped-volume") \
    |> last() \
    |> group(columns: ["registration-id"])`

    var lastReadingDates: { [key: string]: Date } = await new Promise((resolve,reject) => {
        let results
            : { [key: string]: Date }
            = {}
        influxQueryApi.queryRows(query, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                results[o["registration-id"]] = new Date(o["_time"])
            },
            error(error) {
                console.error(error);
                reject(error)
            },
            complete() {
                resolve(results);
            },
        });
    })
    return lastReadingDates;
}

async function cacheWellsInDb(wells: agHubWell[]) {
    let srv = ""
    if (!process.env["DEBUG"]){
        srv = "+srv";
    }

    const connstring = `mongodb${srv}://${config.DATABASE_USER}:${config.DATABASE_PASSWORD}@${config.DATABASE_URI}`;
    const client = await MongoClient.connect(connstring);

    const db = client.db("zybachDb");
    const collection = db.collection("agHubWells");

    await collection.deleteMany({});
    await collection.insertMany(wells);

    await client.close();
}

async function processWell(well: agHubWell) {
    if (well.wellConnectedMeter) {
        const pumpedVolume = await getPumpedVolume(well);
        await writePumpedVolumeIntervals(pumpedVolume.pumpedVolumeTimeSeries, well.wellRegistrationID);
        const k = 8;
    }

}

async function getPumpedVolume(well: agHubWell) {
    const lastReadingDate = lastReadingDates[well.wellRegistrationID];
    let startDateISO: string;
    if (lastReadingDate){
        startDateISO = DateTime.fromJSDate(lastReadingDates[well.wellRegistrationID]).toISODate();
    } else{
        startDateISO = "2019-07-01";
    }
    const endDateISO = DateTime.local().toISODate();

    const pumpedVolumeEndpoint =
    `${config.MAIN_API_BASE_URL}/${well.wellRegistrationID}/pumped-volume/daily-summary?startDateISO=${startDateISO}&endDateISO=${endDateISO}`;
    let result;

    try {
        result = await got.get(pumpedVolumeEndpoint, {
            headers: {
                "x-api-key": config.MAIN_API_KEY
            },
            responseType: "json",
        })
    } catch (err) {
        console.error(err);
        throw err;
    }

    if (result.statusCode !== 200) {
        const message = `Could not get pumped volume for ${well.wellRegistrationID}. ${result.statusCode}: ${result.body.message}`
        console.error(message);
        throw new Error(message);
    }

    return result.body.data;
}

async function writePumpedVolumeIntervals(intervals: { intervaldate: string, pumpedvolumegallons: number }[], wellRegistrationID) {
    let url = `${config.INFLUX_DB_URL}/api/v2/write?org=${config.INFLUX_DB_ORG}&bucket=${config.INFLUX_DB_BUCKET}&precision=ms`;

    const lineProtocolMessage = intervals.reduce((prev, interval) => {
        const timestamp = DateTime.fromISO(interval.intervaldate).toMillis();
        
        return prev + `estimated-pumped-volume,registration-id=${wellRegistrationID} gallons=${interval.pumpedvolumegallons} ${timestamp}\n`;
    }, "");
    let result;

    try {
        result = await got.post(url, {
            body: lineProtocolMessage,
            headers: {
                "Authorization": `Token ${config.INFLUX_DB_TOKEN}`
            }
        });
    } catch (err) {
        console.error(err);
    }
}

main();

interface agHubWell {
    wellRegistrationID: string;
    location: any;
    tpnrdPumpRateUpdated: string;
    wellTpnrdPumpRate: number;
    auditPumpRateUpdated: string;
    wellAuditPumpRate: number;
    wellConnectedMeter: boolean;
    wellTPID: string;
}