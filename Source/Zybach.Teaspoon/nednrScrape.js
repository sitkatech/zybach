// THERE ARE A TOTAL OF 257 PAGES OF WELL DATA
let GetOpt = require('node-getopt');
let fs = require('fs');
let Client = require('node-rest-client').Client;
let Stopwatch = require('statman-stopwatch');

const stopwatch = new Stopwatch();

async function _main() {
    let getopt = new GetOpt([
        ['f', 'folder=ARG'  , 'folder for well info files'],
        ['h', 'help'        , 'display this help'],
    ])
        .bindHelp()     // bind option 'help' to default action
        .parseSystem(); // parse command line
    
    if (!fs.existsSync(getopt.options.folder)) {
        console.error("folder '" + getopt.options.folder + "' not found.");
        return -1;
    }
    let maxPage = parseInt(getopt.options.page) + parseInt(getopt.options.count) - 1;

    let twinPlatteWells = []
    let nextPageUrl = "dummy";
    let page = 1;

    stopwatch.start();
    // when nextPageUrl is zero, we will have asked for a page beyond the number of pages available
    while (nextPageUrl !== null)
    {
        const result = await getWellInformationPage(page);
        const gottenWells = result.wells;
        nextPageUrl = result.nextPageUrl;
        if (nextPageUrl === null){
            // technically this is redundant and continue would have sufficed, or the while loop could be while true and the break could persist;
            // but blah.
            break;
        }

        console.info(`page ${page} retrieved, ${gottenWells.length} wells`);
        page++;

        for (let i = 0; i < gottenWells.length; i++){
            twinPlatteWells.push(gottenWells[i]);
        }
    }
    stopwatch.stop();
    const runningTime = stopwatch.read();
    console.log(`Ran API calls in ${runningTime} milliseconds.`);
    writeWellsToFolder(getopt.options.folder, twinPlatteWells);

    console.info('Done!');
    return 0;
}
async function getWellInformationPage(page) {
    let getPromise = new Promise(function (resolve) {
        let endpoint = `https://nednr.nebraska.gov/IwipApi/api/v1/Wells/AllWells?page=${page}`;
        const args = {data: "", headers: {}};
        let client = new Client();
        client.get(endpoint, args, (body, response) => {
            if (response.statusCode == 200)
                resolve(body);
            else
                handleError(body, response);
        });
    });
    let response = await getPromise;
    const allWells = response.Results;
    return {
        wells: allWells.filter(x => x.RegistrationNumber !== "Removed" && x.NRDName === "Twin Platte"),
        nextPageUrl: response.Links.NextPageUrl
    };

}
function  writeWellsToFolder(folder, wells) {
    // all of the wells for a single NRD will go into a single file
    if (wells.length < 1)
        return;
    
    // super gross building an es6 module as a string but I am going to do it.
    const data = "module.exports = " + JSON.stringify(wells) + ";";
    const nrdFile = `${folder}\\TwinPlatte.js`;
    if (fs.existsSync(nrdFile)){
        fs.unlinkSync(nrdFile);
    }
    fs.writeFileSync(nrdFile, data);

    console.info(`${wells.length} wells written to ${nrdFile}`);
}
function handleError(body, response) {
    throw new Error(`${response.req.method} ${response.responseUrl} returned ${response.statusCode} (${response.statusMessage}`);
}
_main();