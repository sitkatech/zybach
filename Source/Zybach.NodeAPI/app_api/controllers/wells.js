const moment = require('moment');
const secrets =  require('../../secrets');
const { InfluxDB } = require('@influxdata/influxdb-client');

const getPumpedVolume = async (req, res) => {
    const wellRegistrationIDs = req.params.wellRegistrationID || req.query.wellRegistrationIDs;
    const startDateQuery = req.query.startDate;
    const endDateQuery = req.query.endDate ? req.query.endDate : moment(new Date()).format();
    const interval = req.query.interval ? parseInt(req.query.interval) : 60;

    if (wellRegistrationIDs === null || wellRegistrationIDs === undefined) {
        return res.status(400).json({ "status": "invalid request", "reason": "No Well Registration ID included in request. Please include a Well Registration ID." });
    }

    [{ name: "Start Date", value: startDateQuery },
    { name: "End Date", value: endDateQuery }].forEach(x => {
        if (x.value === null || x.value === undefined) {
            return res.status(400).json({ "status": "invalid request", "reason": `${x.name} empty. Please enter a valid ${x.name}.` });
        }

        if (!moment(x.value, "YYYY-MM-DDTHH:mm:ssZ", true).isValid()) {
            return res.status(400).json({ "status": "invalid request", "reason": `${x.name} is not a valid Date string in ISO 8601 format. Please enter a valid date string` });
        }
    });
  
    if (isNaN(interval)) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval is invalid. Please enter an integer evenly divisible by 15 to use for interval." });
    }

    if (interval === 0 || interval % 15 != 0) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval."});
    }

    const startDate = new Date(startDateQuery);
    const endDate =  new Date(endDateQuery);

    if (startDate > endDate) {
        return res.status(400).json({ "status": "invalid request", "reason": "Start date occurs after End date. Please ensure that Start Date occurs before End date"});
    }

    try {
        let results = await getFlowMeterSeries(wellRegistrationIDs, startDateQuery,  endDateQuery);
        return res.status(200).json({ "status": "success", "result": results.length > 0 ? structureResults(results, interval) : results});
    }
    catch (err) {
        return res.status(500).json({ "status": "failed", "result":err });
    }
};

//"G-162367"
//"G-118986"

async function getFlowMeterSeries(wellRegistrationIDs, startDate, endDate) {
    const token = secrets.INFLUXDB_TOKEN;
    const org = secrets.INFLUXDB_ORG;
    const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: token });
    const queryApi = client.getQueryApi(org);

    const registrationIDQuery = `r["registration-id"] == "${Array.isArray(wellRegistrationIDs) ? wellRegistrationIDs.join(`" or r["registration-id"]=="`) : wellRegistrationIDs}"`;
    const query = `from(bucket: "tpnrd") \
        |> range(start: ${startDate}, stop:${endDate}) \
        |> filter(fn: (r) => 
            r["_measurement"] == "gallons" and \
            r["_field"] == "pumped" and \
            ${registrationIDQuery}
        )`;

    let results = [];

    return new Promise((resolve, reject) => {
        queryApi.queryRows(query, {
            next(row, tableMeta) {
                const o = tableMeta.toObject(row);
                results.push({ endTime: new Date(o._time), gallons: o._value, wellRegistrationID: o['registration-id'] });
            },
            error(error) {
                reject(error);
            },
            complete() {
                resolve(results);
            },
        });
    });
}

function structureResults(results, interval) {
    const intervalInMS = interval * 60000;
    const startDate = new Date(results[0].endTime.getTime() - intervalInMS);
    const endDate = results[results.length - 1].endTime;
    const distinctWells = [...new Set(results.map(x => x.wellRegistrationID))];
    let totalResults = 0;
    let volumesByWell = [];
    distinctWells.forEach(wellRegistrationID => {
        let currentWellResults = results.filter(x => x.wellRegistrationID === wellRegistrationID);
        let aggregatedResults = interval > 15 ? aggregateResults(currentWellResults, interval) : currentWellResults;
        totalResults += aggregatedResults.length;
        let finalWellResults = aggregatedResults.filter(x => x.wellRegistrationID == wellRegistrationID).map(x => {
            return {intervalEndTime : x.endTime, gallonsPumped: x.gallons}});
        let newWellObj = {
            wellRegistrationID : wellRegistrationID,
            intervalCount : finalWellResults.length,
            internalVolumes : finalWellResults
        };
        volumesByWell.push(newWellObj);       
    });

    return {
        intervalCountTotal : totalResults,
        intervalWidthInMinutes : interval,
        intervalStart : startDate.toISOString(),
        intervalEnd: endDate.toISOString(),
        durationInMinutes : Math.round((endDate.getTime() - startDate.getTime()) / 60000),
        wellCount : distinctWells.length,
        volumesByWell : volumesByWell
    }
}

function aggregateResults(resultsToAggregate, interval) {
    let aggregatedResults = [];
    let sum = 0;
    let count = 0;
    let startTime = resultsToAggregate[0].endTime
    let timeThreshold = interval * 60 * 1000;
    resultsToAggregate.forEach(x => {
        sum += x.gallons;
        count++;
        if  (x.endTime.getTime() - startTime.getTime() >= timeThreshold) {
            aggregatedResults.push({
                wellRegistrationID: x.wellRegistrationID,
                endTime : x.endTime,
                gallons : sum / count
            })
            sum = 0;
            count = 0;
            startTime = x.endTime;
        }
    })

    return aggregatedResults;
}

module.exports = { getPumpedVolume };