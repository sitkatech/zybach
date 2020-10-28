const { InfluxDB } = require('@influxdata/influxdb-client');
const { Client } = require('node-rest-client');
const {normalizeISOStringTime} = require("../util")

const { influxDBToken, influxDBOrg } = require('../config');
const destinationBucket = process.env.DESTINATION_BUCKET;
const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: influxDBToken });

// queries that generate intervals to write to the normalized time series bucket should draw from a range that stops at exactly XX:00:00.000
const stopTime = normalizeISOStringTime(new Date().toISOString());

// these get*MeterSeries functions have their work wrapped in promises so that we don't proceed without letting all of the rows get processed and pushed to the intervalsToWrite array later
function getContinuityMeterSeries(well) {
    const wellRegistrationID = well.wellRegistrationID;
    const gpm = well.pumpingRate
    const startTime = well.startTime;
    const sn = well.sn;

    return (new Promise((resolve, reject) => {
        const intervalsToWrite = [];
        const queryApi = client.getQueryApi(influxDBOrg);

        /*
            the continuity meter data comes in at seemingly random intervals. this query aggregates data into 15 minute intervals and fills 
            in the missing intervals with null. unlike the flow meters, we can't assume that null means the pump is off, since the meter 
            may have reported on in the prior interval which would mean that the pump is on in the null interval. the rowfunction passed in
            from the caller's job is to replace nulls in this series with the last known non-null value.
    
            another approach would be to query the output series (from the prior run) with a broader range, and then identify the last time
            point in that interval. If you then calculatd the difference between now and that interval, you would know exactly what range
            you needed without having to worry about scheduled runs not working on time, etc.
        */

        const query = `from(bucket: "tpnrd") \
            |> range(start: ${startTime}, stop: ${stopTime}) \
            |> filter(fn: (r) => r["_measurement"] == "continuity") \
            |> filter(fn: (r) => r["_field"] == "on") \
            |> filter(fn: (r) => r["registration-id"] == "${wellRegistrationID}") \
            |> filter(fn: (r) => r["sn"] == "${sn}") \
            |> map(fn: (r) => ({r with _value: r._value * float(v: ${gpm * 15})})) \
            |> aggregateWindow(every: 15m, fn: mean, createEmpty: true) \
            |> fill(usePrevious: true)`;


        queryApi.queryRows(query, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                const toPush = getIntervalFromReturnedRow(o, well.pumpingRate);

                if (toPush.gallons != null) {
                    intervalsToWrite.push(toPush);
                };
            },
            error(error) {
                console.error(error);
                reject();
            },
            complete() {
                resolve(intervalsToWrite);
            },
        });
    }));
}

function getFlowMeterSeries(well) {
    const wellRegistrationID = well.wellRegistrationID;
    const sn = well.sn;
    const startTime = well.startTime;

    return (new Promise((resolve, reject) => {
        const intervalsToWrite = []
        const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: influxDBToken });
        const queryApi = client.getQueryApi(influxDBOrg);

        /*
            the flow meter data comes in 30 minute increments, so this query fills in the missing intervals with 0 and then calculates
            a moving average using a window size of two to average out the 30 minute readings to 15 minute readings.
        */

        const query = `from(bucket: "tpnrd") \
            |> range(start: ${startTime}, stop: ${stopTime}) \
            |> filter(fn: (r) => r["_measurement"] == "gallons") \
            |> filter(fn: (r) => r["_field"] == "pumped") \
            |> filter(fn: (r) => r["registration-id"] == "${wellRegistrationID}") \
            |> filter(fn: (r) => r["sn"] == "${sn}") \
            |> aggregateWindow(every: 15m, fn: mean, createEmpty: true) \
            |> fill(value: 0.0) \
            |> movingAverage(n: 2)`;

        queryApi.queryRows(query, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                intervalsToWrite.push(getIntervalFromReturnedRow(o));
            },
            error(error) {
                console.error(error);
                reject(error);
            },
            complete() {
                resolve(intervalsToWrite);
            },
        });
    }));
}

function getIntervalFromReturnedRow(row) {
    return {
        intervalEndTime: row._time,
        wellRegistrationID: row["registration-id"],
        sn: row.sn,
        gallons: row._value
    }
}

function writePumpedVolumeIntervals(intervals, wellRegistrationID) {
    let url = `https://us-west-2-1.aws.cloud2.influxdata.com/api/v2/write?org=${influxDBOrg}&bucket=${destinationBucket}&precision=ms`;

    const lineProtocolMessage = intervals.reduce((prev, interval) => {
        return prev + getLineFromInterval(interval)
    }, "");

    const args = {
        data: lineProtocolMessage,
        headers: {
            "Authorization": `Token ${influxDBToken}`
        }
    };

    return new Promise(function (resolve, reject) {
        let client = new Client();
        client.post(url, args, (body, response) => {
            if (response.statusCode == 200 || response.statusCode == 204) {
                resolve({
                    wellRegistrationID: wellRegistrationID,
                    status: "success"
                });
            }
            else {
                console.error(body.message);
                console.log(body.message);
                reject(body.message);
            }
        });
    });
}

// todo: this function doesn't need to deal in sets anymore.
async function getWellsWithDataAsOf(startTime) {
    const flowMeterQuery = `from(bucket: "tpnrd") \
            |> range(start: ${startTime}) \
            |> filter(fn: (r) => r["_measurement"] == "gallons") \
            |> filter(fn: (r) => r["_field"] == "pumped") \
            |> count() \
            |> group(columns: ["registration-id"])`;


    const continuityMeterQuery = `from(bucket: "tpnrd") \
            |> range(start: ${startTime}) \
            |> filter(fn: (r) => r["_measurement"] == "continuity") \
            |> filter(fn: (r) => r["_field"] == "on") \
            |> count() \
            |> group(columns: ["registration-id"])`;

    const queryApi = client.getQueryApi(influxDBOrg);

    const flowMeterPromise = new Promise((resolve, reject) => {
        const wellRegistrationIDs = new Set();

        queryApi.queryRows(flowMeterQuery, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                const wellRegistrationID = o["registration-id"];
                wellRegistrationIDs.add(wellRegistrationID);
            },
            error(error) {
                console.error(error);
                reject();
            },
            complete() {
                const wells = Array.from(wellRegistrationIDs).map(x=>{ return {
                    wellRegistrationID: x,
                    sensorType: "flow",
                    startTime: startTime
                }});
                resolve(wells);
            }
        });
    });
    const continuityMeterPromise = new Promise((resolve, reject) => {
        const wellRegistrationIDs = new Set();
        queryApi.queryRows(continuityMeterQuery, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                const wellRegistrationID = o["registration-id"];
                wellRegistrationIDs.add(wellRegistrationID);
            },
            error(error) {
                console.error(error);
                reject();
            },
            complete() {
                const wells = Array.from(wellRegistrationIDs).map(x=>{ return {
                    wellRegistrationID: x,
                    sensorType: "continuity",
                    startTime: startTime
                }});
                resolve(wells);
            }
        });
    });

    const flowWells = await flowMeterPromise;
    const continuityWells = await continuityMeterPromise;

    return new Set([...flowWells, ...continuityWells])
}

// todo: finish implement 
async function getWellsWithEarliestTimestamps(){
    // todo: should there not just be one global queryApi for this swervice? 
    const queryApi = client.getQueryApi(influxDBOrg);

    const flowMeterQuery = `from(bucket: "tpnrd") \
            |> range(start: 2000-01-01T00:00:00Z) \
            |> filter(fn: (r) => r["_measurement"] == "gallons") \
            |> filter(fn: (r) => r["_field"] == "pumped") \
            |> first() \
            |> group(columns: ["registration-id", "sn"])`;

    const continuityMeterQuery = `from(bucket: "tpnrd") \
    |> range(start: 2000-01-01T00:00:00Z) \
    |> filter(fn: (r) => r["_measurement"] == "continuity") \
    |> filter(fn: (r) => r["_field"] == "on") \
    |> first() \
    |> group(columns: ["registration-id", "sn"])`;

    const continuityMeterPromise = new Promise((resolve, reject) => {
        const wells = []

        queryApi.queryRows(continuityMeterQuery, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                wells.push({
                    wellRegistrationID: o["registration-id"],
                    sensorType: "continuity",
                    startTime: normalizeISOStringTime(o["_time"]),
                    sn: o["sn"]
                });
            },
            error(error) {
                console.error(error);
                reject();
            },
            complete() {
                resolve(wells);
            }
        });
    });

    const flowMeterPromise = new Promise((resolve, reject) => {
        const wells = []

        queryApi.queryRows(flowMeterQuery, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                wells.push({
                    wellRegistrationID: o["registration-id"],
                    sensorType: "flow",
                    startTime: normalizeISOStringTime(o["_time"]),
                    sn: o["sn"]
                });
            },
            error(error) {
                console.error(error);
                reject();
            },
            complete() {
                resolve(wells);
            }
        });
    });

    const continuities = await continuityMeterPromise;
    const flows = await flowMeterPromise;
    return [...continuities, ...flows];
}

function getLatestTimestampForGivenWell(endTime = null){

}

function getLineFromInterval(interval) {
    // IMPORTANT: Date.parse yields a timestamp measured in milliseconds since the Unix epoch whereas the default precision for InfluxDB is
    // nanoseconds since the epoch. If you pass a Date.parse() value into a write without the "precision=ms" query parameter, InfluxDB will 
    // think the value is in nanoseconds and will place the event much farther back in time than you expect. If you try to query the value using
    // a range you think should work but doesn't work, check this first.

    let timestamp = Date.parse(interval.intervalEndTime);

    return `pumped-volume,registration-id=${interval.wellRegistrationID},sn=${interval.sn} gallons=${interval.gallons} ${timestamp}\n`;
};

module.exports.getContinuityMeterSeries = getContinuityMeterSeries;
module.exports.getFlowMeterSeries = getFlowMeterSeries;
module.exports.writePumpedVolumeIntervals = writePumpedVolumeIntervals;
module.exports.getWellsWithDataAsOf = getWellsWithDataAsOf;
module.exports.getWellsWithEarliestTimestamps = getWellsWithEarliestTimestamps;