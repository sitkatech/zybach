const moment = require('moment');
const { InfluxDB } = require('@influxdata/influxdb-client');

const token = process.env.INFLUXDB_TOKEN;
const org = process.env.INFLUXDB_ORG;

const getPumpedVolume = async (req, res) => {
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
        return res.status(400).json({ "status": "invalid request", "reason": "Interval must be a number evenly divisible by 15 and greater than 0. Please enter a new interval."});
    }

    const startDate = new Date(startDateQuery);
    const endDate =  new Date(endDateQuery);

    if (startDate > endDate) {
        return res.status(400).json({ "status": "invalid request", "reason": "Start date occurs after End date. Please ensure that Start Date occurs before End date"});
    }

    try {
        let results = await getFlowMeterSeries(req.params.wellid, startDateQuery,  endDateQuery);
        //Don't do any processing if we don't have to
        if (interval == 15 || results.length == 0) {
            return res.status(200).json({ "status": "success", "resultCount": results.length, "result": results});
        }

        let aggregatedResults = [];
        let sum = 0;
        let count = 0;
        let startTime = results[0].endTime
        let timeThreshold = interval * 60 * 1000;
        results.forEach(x => {
            sum += x.gallons;
            count++;
            if  (x.endTime.getTime() - startTime.getTime() >= timeThreshold) {
                aggregatedResults.push({
                    endTime : x.endTime,
                    gallons : sum / count
                })
                sum = 0;
                count = 0;
                startTime = x.endTime;
            }
        })

        return res.status(200).json({ "status": "success", "resultCount": aggregatedResults.length, "result": aggregatedResults});
    }
    catch (err) {
        return res.status(500).json({ "status": "failed", "result":err });
    }
};

//"G-162367"
//"G-118986"

async function getFlowMeterSeries(wellRegistrationID, startDate, endDate) {
    const client = new InfluxDB({ url: 'https://us-west-2-1.aws.cloud2.influxdata.com', token: token });
    const queryApi = client.getQueryApi(org);

    /*
        the flow meter data comes in 30 minute increments, so this query fills in the missing intervals with 0 and then calculates
        a moving average using a window size of two to average out the 30 minute readings to 15 minute readings.
    */

    const query = `from(bucket: "tpnrd") \
        |> range(start: ${startDate}, stop:${endDate}) \
        |> filter(fn: (r) => 
            r["_measurement"] == "gallons" and \
            r["_field"] == "pumped" and \
            r["registration-id"] == "${wellRegistrationID}"
        ) \
        |> aggregateWindow(every: 15m, fn: mean, createEmpty: true) \
        |> fill(value: 0.0) \
        |> movingAverage(n: 2)`;

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

module.exports = { getPumpedVolume };