const moment = require('moment');
const secrets = require('../../secrets');
const geoOptixService = require('../services/geo-optix-token-service');
const { InfluxDB } = require('@influxdata/influxdb-client');
const axios = require('axios');
const { start } = require('applicationinsights');

const bucketName = process.env.SOURCE_BUCKET;

const getPumpedVolume = async (req, res) => {
    const wellRegistrationIDs = req.params.wellRegistrationID || req.query.filter;
    const startDateQuery = req.query.startDate;
    const endDateQuery = req.query.endDate ? req.query.endDate : moment(new Date()).format();
    const interval = req.query.interval ? parseInt(req.query.interval) : 60;

    [{ name: "Start Date", value: startDateQuery },
    { name: "End Date", value: endDateQuery }].forEach(x => {
        if (x.value === null || x.value === undefined) {
            return res.status(400).json({ "status": "invalid request", "reason": `${x.name} empty. Please enter a valid ${x.name}.` });
        }

        if (!moment(x.value, "YYYY-MM-DD",true).isValid() && !moment(x.value, "YYYYMMDD", true).isValid()
            && !moment(x.value, "YYYYMMDDTHHmmssZ", true).isValid() && !moment(x.value, "YYYY-MM-DDTHH:mm:ssZ", true).isValid()
            ) {
            return res.status(400).json({ "status": "invalid request", "reason": `${x.name} is not a valid Date string in ISO 8601 format. Please enter a valid date string` });
        }
    });

    if (isNaN(interval)) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval is invalid. Please enter an integer evenly divisible by 15 to use for interval." });
    }

    if (interval === 0 || interval % 15 != 0) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval." });
    }

    const startDate = moment(startDateQuery).toDate();
    const endDate = moment(endDateQuery).toDate();

    if (startDate > endDate) {
        return res.status(400).json({ "status": "invalid request", "reason": "Start date occurs after End date. Please ensure that Start Date occurs before End date" });
    }

    try {
        let results = await getFlowMeterSeries(wellRegistrationIDs, startDate, endDate);
        return res.status(200).json({ "status": "success", "result": results.length > 0 ? structureResults(results, interval) : results });
    }
    catch (err) {
        console.error(err);
        return res.status(500).json({ "status": "failed", "result": err.message });
    }
};

async function getFlowMeterSeries(wellRegistrationIDs, startDate, endDate) {
    const token = secrets.INFLUXDB_TOKEN;
    const org = secrets.INFLUXDB_ORG;
    const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: token });
    const queryApi = client.getQueryApi(org);

    const fifteenMinutesInms = 1000  * 60 * 15;
    const startDateForFlux = new Date((Math.round(startDate.getTime() / fifteenMinutesInms) * fifteenMinutesInms) + 1000).toISOString();
    const endDateForFlux = new Date((Math.round(endDate.getTime() / fifteenMinutesInms) * fifteenMinutesInms) + 1000).toISOString();

    const registrationIDQuery = wellRegistrationIDs !== null && wellRegistrationIDs != undefined ? `and r["registration-id"] == "${Array.isArray(wellRegistrationIDs) ? wellRegistrationIDs.join(`" or r["registration-id"]=="`) : wellRegistrationIDs}"` : "";
    const query = `from(bucket: "${bucketName}") \
        |> range(start: ${startDateForFlux}, stop:${endDateForFlux}) \
        |> filter(fn: (r) => 
            r["_measurement"] == "pumped-volume" and \
            r["_field"] == "gallons" \
            ${registrationIDQuery}) \
        |> sort(columns:["_time"])`;

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
    const distinctWells = [...new Set(results.map(x => x.wellRegistrationID))];
    let startDate = results[0].endTime;
    let endDate = results[results.length - 1].endTime;
    let totalResults = 0;
    let volumesByWell = [];
    distinctWells.forEach(wellRegistrationID => {
        let currentWellResults = results.filter(x => x.wellRegistrationID === wellRegistrationID).sort((a, b) => a.endTime -  b.endTime);
        
        if (currentWellResults[0].endTime < startDate) {
            startDate = currentWellResults[0].endTime;
        }

        let aggregatedResults = aggregateResults(currentWellResults, interval);
        
        if (aggregatedResults[aggregatedResults.length-1].endTime > endDate) {
            endDate = aggregatedResults[aggregatedResults.length-1].endTime;
        }

        totalResults += aggregatedResults.length;

        let newWellObj = {
            wellRegistrationID: wellRegistrationID,
            intervalCount: aggregatedResults.length,
            intervalVolumes: aggregatedResults
        };
        volumesByWell.push(newWellObj);
    });

    //Because we get the intervals back in 15 minute increments, technically our startDate is 15 minutes BEFORE our actual first time
    //Remove this extra piece if we decide we just want the first interval's end date
    startDate = new Date(startDate.getTime() - (15 * 60000));

    return {
        intervalCountTotal: totalResults,
        intervalWidthInMinutes: interval,
        intervalStart: startDate.toISOString(),
        intervalEnd: endDate.toISOString(),
        durationInMinutes: Math.round((endDate.getTime() - startDate.getTime()) / 60000),
        wellCount: distinctWells.length,
        volumesByWell: volumesByWell
    }
}

function aggregateResults(resultsToAggregate, interval) {
    let aggregatedResults = [];
    let sum = 0;
    //Again, because of the 15 minute intervals the first date we get will have been over a previous 15 minute interval
    //So, when we start aggregating, we need to have our first start time 15 minutes BEFORE our first endTime
    let startTime = new Date(resultsToAggregate[0].endTime.getTime() - (15 * 60000));
    let timeThreshold = interval * 60 * 1000;
    resultsToAggregate.forEach(x => {
        sum += Math.round(x.gallons);
        if (x.endTime.getTime() - startTime.getTime() >= timeThreshold) {
            aggregatedResults.push({
                intervalEndTime: x.endTime,
                volumePumpedGallons: sum
            })
            sum = 0;
            startTime = x.endTime;
        }
    })

    //TODO:If we have an incomplete interval, do we want to push it? 
    //Or should we leave incomplete intervals out of the payload? 
    //Should there be a marker stating that it's incomplete?
    if (sum > 0) {
        aggregatedResults.push({
            wellRegistrationID: resultsToAggregate[resultsToAggregate.length - 1].wellRegistrationID,
            endTime: resultsToAggregate[resultsToAggregate.length - 1].endTime,
            gallons: sum
        })
    }

    return aggregatedResults;
}

function abbreviateWellDataResponse(wellData) {
    return {
        wellRegistrationID: wellData.CanonicalName,
        description: wellData.Description,
        tags: wellData.Tags,
        location: wellData.Location,
        createDate: wellData.CreateDate,
        updateDate: wellData.UpdateDate
    }
}

function abbreviateWellSensorsResponse(wellSensors) {
    return wellSensors.map(x => ({
        sensorName: x.CanonicalName,
        sensorType: x.Definition.sensorType
    }));
}

const getWells = async (req, res) => {
    try {
        const geoOptixAccessToken = await geoOptixService.getGeoOptixAccessToken();
        const geoOptixRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites`, {
            headers: {
                "Authorization": `Bearer ${geoOptixAccessToken}`
            }
        });
        return res.status(200).json({
            "status": "success",
            "result": geoOptixRequest.data.map(x => ({wellRegistrationID : x.CanonicalName, description : x.Description, location: x.Location}))
        });
    }
    catch (err) {
        console.error(err);
        return res.status(500).json({ "status": "failed", "result": err.message });
    }
}

const getWell = async (req, res) => {
    try {
        const wellRegistrationID = req.params.wellRegistrationID;
        const geoOptixAccessToken = await geoOptixService.getGeoOptixAccessToken();
        const geoOptixWellRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}`, {
            headers: {
                "Authorization": `Bearer ${geoOptixAccessToken}`
            }
        });
        let resultsObject = abbreviateWellDataResponse(geoOptixWellRequest.data);
        const geoOptixWellSensorsRequest = await axios.get(`${secrets.GEOOPTIX_HOSTNAME}/project-overview-web/water-data-program/sites/${wellRegistrationID}/stations`, {
            headers: {
                "Authorization": `Bearer ${geoOptixAccessToken}`
            }
        });
        resultsObject["sensors"] = abbreviateWellSensorsResponse(geoOptixWellSensorsRequest.data);
        return res.status(200).json({
            "status": "success",
            "result": resultsObject
        });
    }
    catch (err) {
        console.error(err);
        return res.status(500).json({ "status": "failed", "result": err.message });
    }
}

module.exports = { getPumpedVolume, getWells, getWell };