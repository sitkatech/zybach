const debug = require('debug');
const {getFlowMeterSeries,getContinuityMeterSeries,writePumpedVolumeIntervals, getWellsWithDataAsOf, getWellsWithEarliestTimestamps} = require('./influxService');
const getGpmFromNednrAPI = require('./nednrService');
let GetOpt = require('node-getopt');
let Stopwatch = require('statman-stopwatch');

const stopwatch = new Stopwatch();
debug.log = console.debug.bind(console);

process.on('unhandledRejection', console.log.bind(console));

function processWell(well) {
    return (new Promise((resolve) => {
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

        seriesProcessedPromise.then(res => {
            if (!res.length){
                //console.log(`No points for ${well.wellRegistrationID}`);
                return Promise.resolve({
                    wellRegistrationID:well.wellRegistrationID,
                    status:"error",
                    reason:"nodata"
                });
            }
            return writePumpedVolumeIntervals(res, well.wellRegistrationID);
        // todo: resolve a more useful object for logging.
        }).then(res => {
            res.sensorType = well.sensorType;
            resolve(res);
        }).catch(()=>{
            //console.log(`Strange error for ${well.wellRegistrationID}`);
            resolve({
                wellRegistrationID: well.wellRegistrationID,
                sensorType: well.sensorType,
                status: "error",
                reason:"strange"
            });
        });
    }));
}

async function readyDebug() {
    await new Promise(resolve => setTimeout(resolve, 2000));
    debug("TSPM Ready");
}

async function assignPumpingRate(continuityWell){
    const pumpingRate = await getGpmFromNednrAPI(continuityWell.wellRegistrationID);

    continuityWell.pumpingRate = pumpingRate
    //console.log(`Assigned pumping rate to Well ${continuityWell.wellRegistrationID}`);
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
    console.log("Assigning pumping rates to continuity wells");
    stopwatch.start();
    for (let i = 0; i < continuityWellsWithNewData.length; i++) {
        await assignPumpingRate(continuityWellsWithNewData[i]);
    }
    stopwatch.stop();
    console.log("Done assigining pumping rates to continuity wells");
    console.log(`(Took ${stopwatch.read()} ms)`);

    stopwatch.reset();

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

    console.log(results);
    console.log(results.filter(x => x.sensorType == 'flow' && x.status == 'error').length + " erroring flows");
    console.log(results.filter(x => x.sensorType == 'continuity' && x.status == 'error').length + " erroring continuities");
}

async function completeProcessing(){
    const wellsWithEarliestTimestamp = Array.from(await getWellsWithEarliestTimestamps());
    console.log(wellsWithEarliestTimestamp);
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
    
    console.log(`Begin run at ${new Date()}`);

    if (getopt.options.complete) {
        completeProcessing();
    } else{
        incrementalProcessing();
    }
 
    console.log(`End run at ${new Date()}`);
}

main();