const {APPINSIGHTS_INSTRUMENTATIONKEY} = require('./config');
delete process.env["APPLICATION_INSIGHTS_NO_DIAGNOSTIC_CHANNEL"];
const appInsights = require('applicationinsights');
appInsights.setup(APPINSIGHTS_INSTRUMENTATIONKEY)
          .setAutoCollectConsole(true, true)
          .start();
const client = appInsights.defaultClient;

const {getFlowMeterSeries,getContinuityMeterSeries,writePumpedVolumeIntervals, getWellsWithDataAsOf, getWellsWithEarliestTimestamps} = require('./influxService');
const getGpmFromNednrAPI = require('./nednrService');
let GetOpt = require('node-getopt');
let Stopwatch = require('statman-stopwatch');

const stopwatch = new Stopwatch();

function processWell(well) {
    return (new Promise(async (resolve) => {
        /* 
            the range parameter of the "get*MeterSeries()" calls is relative to right now and can be expressed in a wide variety 
            of time scales (seconds, minutes, hours, days, etc.). the ideal range choice would be long enough in duration to cover
            the time period between the last known interval processed by this code and right now. iow, if this function were to run
            once per hour, we would want a duration of -1h (plus a buffer to make sure we're not missing observations).
        */

        let seriesProcessedPromise;
        if (well.sensorType == 'flow') {
            seriesProcessedPromise = getFlowMeterSeries(well);
        }
        else if (well.sensorType == 'continuity') {
            seriesProcessedPromise = getContinuityMeterSeries(well);
        }

        let intervals;

        try {
            intervals = await seriesProcessedPromise;
            
            if (!intervals.length){
                resolve({
                    wellRegistrationID:well.wellRegistrationID,
                    status:"error",
                    reason:"nodata",
                    sensorType: well.sensorType
                })
                return;
            }

            const result = await writePumpedVolumeIntervals(intervals, well.wellRegistrationID);
            resolve(result);
            return;
        } catch (error) {
            resolve({
                wellRegistrationID: well.wellRegistrationID,
                sensorType: well.sensorType,
                status: "error",
                reason:"strange",
                message: error
            });

            return;
        }

        // seriesProcessedPromise.then(res => {
        //     if (!res.length){
        //         return Promise.resolve({
        //             wellRegistrationID:well.wellRegistrationID,
        //             status:"error",
        //             reason:"nodata"
        //         });
        //     }
        //     return writePumpedVolumeIntervals(res, well.wellRegistrationID);
        // }).then(res => {
        //     res.sensorType = well.sensorType;
        //     resolve(res);
        // }).catch(()=>{
            
        // });
    }));
}

async function readyDebug() {
    await new Promise(resolve => setTimeout(resolve, 2000));
    console.log("TSPM Ready");
}

async function assignPumpingRate(continuityWell){
    const pumpingRate = await getGpmFromNednrAPI(continuityWell.wellRegistrationID);

    continuityWell.pumpingRate = pumpingRate
}

async function incrementalProcessing() {
    // minutes and seconds so that 15min intervals are what they should be
    const startTime = new Date();
    startTime.setHours(startTime.getHours() - 3);
    startTime.setMinutes(0);
    startTime.setSeconds(0);
    startTime.setMilliseconds(0);

    console.log(startTime);

    // this is the mock collection of wells that have had data come in over since we last checked
    const wellsWithNewData = Array.from(await getWellsWithDataAsOf(startTime.toISOString()));

    // await the result of getting and assigning the pumping rate for each well.
    // this keeps the asynchronous calls to the pumping rate API sequentialized 
    // and untangled from the influxDB work that starts after. Log statements exist
    // to demonstrate that everything happens in the order we expect.
    await assignPumpingRatesAndProcessWells(wellsWithNewData);
}

async function assignPumpingRatesAndProcessWells(wellsToProcess) {
    const continuityWellsWithNewData = wellsToProcess.filter(x => x.sensorType === 'continuity');
    for (let i = 0; i < continuityWellsWithNewData.length; i++) {
        await assignPumpingRate(continuityWellsWithNewData[i]);
    }

    // will work through the wells and their processing as promises, only allowing one well's
    // promise to be created after the previous one is resolved, so our task queue will stay
    // lean.
    // await wellsWithNewData.reduce((previousPromise, nextWell) => previousPromise.then(()=>{
    //     processWell(nextWell).then(log);
    // }),
    // emptyPromise);
    const results = [];
    // I think this serializes the promises but I'm not sure?
    for (let i = 0; i < wellsToProcess.length; i++) {
        const result = await processWell(wellsToProcess[i]);
        results.push(result);
    }

    const errors = results.filter(x=>x.status === "error");

    if (errors && errors.length){
        console.error(`Encountered errors on ${errors.length} wells!`);
        errors.forEach(e=>console.error(e));
    }
}

async function completeProcessing(){
    const wellsWithEarliestTimestamp = Array.from(await getWellsWithEarliestTimestamps());
    await assignPumpingRatesAndProcessWells(wellsWithEarliestTimestamp);
}

async function main(){
    // debugging isn't exactly working anymore now that I've switched over to docker-compose for parity with QA
    // await readyDebug();

    let getopt = new GetOpt([
        ['c', 'complete'  , 're-run from earliest available data for all wells'],
        ['h', 'help'      , 'display this help'],
    ])
        .bindHelp()     // bind option 'help' to default action
        .parseSystem(); // parse command line
    
    if (getopt.options.complete) {
        completeProcessing();
    } else{
        incrementalProcessing();
    }
}

main();