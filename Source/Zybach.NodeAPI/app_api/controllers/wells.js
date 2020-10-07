const moment = require('moment');
const secrets = require('../../secrets');
const { InfluxDB } = require('@influxdata/influxdb-client');
const { poolPromise } = require('../../db')
const sql = require('mssql');
const axios = require('axios');
const querystring = require('querystring');

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

        if (!moment(x.value, "YYYY-MM-DDTHH:mm:ssZ", true).isValid()) {
            return res.status(400).json({ "status": "invalid request", "reason": `${x.name} is not a valid Date string in ISO 8601 format. Please enter a valid date string` });
        }
    });

    if (isNaN(interval)) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval is invalid. Please enter an integer evenly divisible by 15 to use for interval." });
    }

    if (interval === 0 || interval % 15 != 0) {
        return res.status(400).json({ "status": "invalid request", "reason": "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval." });
    }

    const startDate = new Date(startDateQuery);
    const endDate = new Date(endDateQuery);

    if (startDate > endDate) {
        return res.status(400).json({ "status": "invalid request", "reason": "Start date occurs after End date. Please ensure that Start Date occurs before End date" });
    }

    try {
        let results = await getFlowMeterSeries(wellRegistrationIDs, startDateQuery, endDateQuery);
        return res.status(200).json({ "status": "success", "result": results.length > 0 ? structureResults(results, interval) : results });
    }
    catch (err) {
        console.error(err);
        return res.status(500).json({ "status": "failed", "result": err.message });
    }
};

//"G-162367"
//"G-118986"

async function getGeoOptixAccessToken() {
    return new Promise(async function (resolve, reject) {
        try {
            const pool = await poolPromise
            const getAccessTokenResult = await pool.request()
                .query('select top 1 * from dbo.GeoOptixAccessToken');
            let currentToken = getAccessTokenResult.recordset.length > 0 ? getAccessTokenResult.recordset[0].GeoOptixAccessTokenValue : null;
            if (currentToken == null || ((new Date(currentToken).getTime() - new Date().getTime()) / 1000 * 60 * 60) < 2) {
                try {
                    const newTokenRequest = await makeKeystoneTokenRequest();
                    if (currentToken != null) {
                        await deleteTableRecords();
                    }
                    await insertNewTokenIntoDatabase(newTokenRequest.data);
                    currentToken = newTokenRequest.data.access_token;
                }
                catch (err) {
                    reject(err);
                }
            }
            resolve(currentToken);
        } catch (err) {
            console.log(err.message);
            reject(err.message);
        }
    });
}

async function deleteTableRecords() {
    return new Promise(async function (resolve, reject) {
        const pool = await poolPromise
        const result = await pool.request()
            .query('delete from dbo.GeoOptixAccessToken', async function (err, results) {
                if (err) {
                    reject(err);
                }
                else {
                    resolve(results);
                }
            })
    });
}

async function insertNewTokenIntoDatabase(newTokenDataObject) {
    return new Promise(async function (resolve, reject) {
        try {
            const pool = await poolPromise
            const result = await pool.request()
                .input('newToken', sql.VarChar(2048), newTokenDataObject.access_token)
                .input('expiryDate', sql.DateTime, new Date(new Date().getTime() + newTokenDataObject.expires_in * 1000))
                .query('insert into dbo.GeoOptixAccessToken (GeoOptixAccessTokenValue, GeoOptixAccessTokenExpiryDate) values (@newToken, @expiryDate)');
            resolve(result);
        }
        catch (err) {
            reject(err);
        }
    });
}

async function makeKeystoneTokenRequest() {
    return new Promise(async function (resolve, reject) {
        axios.post("https://qa.keystone.sitkatech.com/core/connect/token",
            querystring.stringify({
                client_id: 'ZybachApiAccess', //gave the values directly for testing
                client_secret: 'CBE88174-D110-4B27-83CC-8EE2280CE9EB',
                scope: "openid all_claims keystone",
                grant_type: "password",
                username: "ZybachGeoOptixUser",
                password: "O7iqve#HwW66l1uU"
            }), {
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            }
        }).then(function (response) {
            resolve(response);
        }).catch(error => {
            console.error(error);
            reject(error);
        })
    });
}

async function getFlowMeterSeries(wellRegistrationIDs, startDate, endDate) {
    const token = secrets.INFLUXDB_TOKEN;
    const org = secrets.INFLUXDB_ORG;
    const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: token });
    const queryApi = client.getQueryApi(org);

    const registrationIDQuery = wellRegistrationIDs !== null && wellRegistrationIDs != undefined ? `and r["registration-id"] == "${Array.isArray(wellRegistrationIDs) ? wellRegistrationIDs.join(`" or r["registration-id"]=="`) : wellRegistrationIDs}"` : "";
    const query = `from(bucket: "tpnrd-qa") \
        |> range(start: ${startDate}, stop:${endDate}) \
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

function structureResults(resultsIn, interval) {
    const results = resultsIn.sort((a, b) => a.endTime - b.endTime);
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
            return { intervalEndTime: x.endTime, gallonsPumped: x.gallons }
        });
        let newWellObj = {
            wellRegistrationID: wellRegistrationID,
            intervalCount: finalWellResults.length,
            internalVolumes: finalWellResults
        };
        volumesByWell.push(newWellObj);
    });

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
    let count = 0;
    let startTime = resultsToAggregate[0].endTime
    let timeThreshold = interval * 60 * 1000;
    resultsToAggregate.forEach(x => {
        sum += x.gallons;
        count++;
        if (x.endTime.getTime() - startTime.getTime() >= timeThreshold) {
            aggregatedResults.push({
                wellRegistrationID: x.wellRegistrationID,
                endTime: x.endTime,
                gallons: sum / count
            })
            sum = 0;
            count = 0;
            startTime = x.endTime;
        }
    })

    return aggregatedResults;
}

const getWells = async (req, res) => {
    try {
        const geoOptixAccessToken = await getGeoOptixAccessToken();
        const geoOptixRequest = await axios.get("https://tpnrd.api-qa.geooptix.com/project-overview-web/water-data-program/sites", {
            headers: {
                "Authorization": `Bearer ${geoOptixAccessToken}`
            }
        });
        return res.status(200).json({
            "status": "success", 
            "result": geoOptixRequest.data
        });
    }
    catch (err) {
        console.error(err);
        return res.status(500).json({ "status": "failed", "result": err.message });
    }
}

module.exports = { getPumpedVolume, getWells };