/*

this is what an incoming event message looks like:

{
    "id":"867de240-18d0-430e-bfce-7c0df6d35ad7",
    "subject":"stations/PW010090",
    "data": {
        "status": {
            "dateTime":"2020-12-30T21:41:23.6059605Z",
            "status":2,
            "messageCode": "SensorNotFound",
            "message":"Message received for station that is not configured in GeoOptix."
        },
        "properties": {
            "tenantID":"tpnrd",
            "sensorType":"Paige.Message.PumpMonitor",
            "serialNumber":"PW010090",
            "eui":"00008888ccccaaaa"
        }
    },
    "eventType":"GeoOptix.Functions.PaigeWirelessGateway",
    "dataVersion":"1",
    "metadataVersion":"1",
    "eventTime":"2020-12-30T20:42:23.4904986Z",
    "topic":"/subscriptions/aa889d8b-9ad4-4355-9709-888b35e37bc7/resourceGroups/geooptix-prod-rg/providers/Microsoft.EventGrid/topics/paigewireless-status-prod"
}

*/

const STATUS_OK = 0
const STATUS_WARNING = 1
const STATUS_ERROR = 2

const appInsights = require('applicationinsights');
const settings = require('./config.json');
const got = require('got');
let requestContext = null;

// prints a list of boards available to the user that the api key is mappped to

async function printBoard(boardName) {

    let boards_route = `https://api.trello.com/1/members/me/boards?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const boards = await got(boards_route, {headers: { "Accept": "application/json"}}).json();

        boards.forEach(async (b) => {
            if (b.name == boardName) {
                console.info(b);
            }
        });
    }
    catch (error)
    {
        console.error(error);
    }
}

// prints a list of all the lists within a given board

async function printLists(boardId) {

    let lists_route = `https://api.trello.com/1/boards/${boardId}/lists?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const lists = await got(lists_route, {headers: { "Accept": "application/json"}}).json();

        lists.forEach((l) => {
            console.info(l);        
        });
    }
    catch (error)
    {
        console.error(error);
    }
}

// prints the list of custom fields for a specific board

async function printCustomFieldIds(boardId) {
    
    let fields_route = `https://api.trello.com/1/boards/${boardId}/customFields?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const fields = await got(fields_route, {headers: { "Accept": "application/json"}}).json();

        fields.forEach((f) => {
            console.info(`id=${f.id}, name=${f.name}`);   
        });
    }
    catch (error)
    {
        console.error(error);
    }
}

// prints a list of valid values for custom dropdown "option" fields

async function printCustomFieldOptions(fieldId) {

    let options_route = `https://api.trello.com/1/customFields/${fieldId}/options?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const options = await got(options_route, {headers: { "Accept": "application/json"}}).json();

        options.forEach((o) => {
            console.info(`id=${o._id}, name=${o.value.text}`);            
        });
    }
    catch (error)
    {
        console.error(error);
    }
}

// find the first card with a matching name and origin in one of the un-resolved lists

async function findCardByNameAndOrigin(board, name, origin) {

    const route = `https://api.trello.com/1/search?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}&idBoards=${board}&modelTypes=cards&card_fields=name&card_list=true&query=name:${name}`

    try {
        const body = await got(route, { headers: {"Accept": "application/json"} }).json();
        const cards = body.cards;
        
        let matchedCard = null;

        for (let i = 0; i < cards.length; i++) {
            
            // skip any card in the resolved list
            if (cards[i].idList == settings.trello.lists.resolved)
                continue;
           
            // matching card name, now see if the origin matches
            let originValue = await getCustomFieldValue(cards[i].id, settings.trello.customFields.origin);
            
            if (originValue.status == 200 && originValue.value == origin)
            {
                matchedCard = {
                    id: cards[i].id,
                    originId: originValue.value
                };
            
                // grab the error count as well
                let errorValue = await getCustomFieldValue(cards[i].id, settings.trello.customFields.errorCount);

                matchedCard.errorCount = errorValue.value;
            }
        }

        return { status: 200, card: matchedCard };
    }
    catch (error)
    {
        logException(error);
        return {status: 500 };
    }
}

// from the information received in the event body, create a new card in the "unassigned" list

async function updateCustomField(cardID, fieldID, fieldValue) {

    let route =  `https://api.trello.com/1/cards/${cardID}/customField/${fieldID}/item?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const response = await got.put(`${route}`, 
            {
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                json: fieldValue
            });

        return {status: response.statusCode }; 
    }
    catch (error)
    {
        logException(error);
        return {status: 500 };
    }
}

// this function returns the normalized value of a custom field for a specific card

async function getCustomFieldValue(cardID, customFieldID) {

    let route =  `https://api.trello.com/1/cards/${cardID}/customFieldItems?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const body = await got(`${route}`, { headers: { "Accept": "application/json" } }).json();
        let fieldValue = 1;

        for (let i = 0; i < body.length; i++) {
            let field = body[i];

            if (field.idCustomField == customFieldID) {
                if (field.hasOwnProperty('idValue'))
                {
                    // option list
                    fieldValue = field.idValue
                }
                else
                {
                    // scalar values
                    if (field.value.hasOwnProperty('number'))
                        fieldValue = field.value.number;
                    else if (field.value.hasOwnProperty('text'))
                        fieldValue = field.value.text;
                    else if (field.value.hasOwnProperty('date'))
                        fieldValue = field.value.date;
                    else
                        fieldValue = 'Unsupported type';
                }
                break;
            }
        }
 
        return { status: 200, value: fieldValue }; 
    }
    catch (error)
    {
        logException(error);
        return {status: 500 };
    }
}

// creates a brand new card in the Unassigned list and sets custom field values on that card

async function createUnassignedCard(wellNumber, serialNumber, location, sensorType, errorMessage, origin) {

    let name = wellNumber || serialNumber;
    let cardRoute =  `https://api.trello.com/1/cards?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}&name=${name}&desc=${errorMessage}&idList=${settings.trello.lists.unassigned}`;
    
    if (location) {
        cardRoute = cardRoute.concat(`&coordinates=${location.latitude},${location.longitude}`)
    }

    try {
        let card = await got.post(`${cardRoute}`, { headers: {"Accept": "application/json"}}).json();
 
        await updateCustomField(card.id, settings.trello.customFields.registration, { value: { text: wellNumber }});
        await updateCustomField(card.id, settings.trello.customFields.serial, { value: { text: serialNumber }});
        await updateCustomField(card.id, settings.trello.customFields.sensorType, { idValue: sensorType });
        await updateCustomField(card.id, settings.trello.customFields.errorMessage, { value: { text: errorMessage }});
        await updateCustomField(card.id, settings.trello.customFields.errorCount, { value: { number: "1" }});
        await updateCustomField(card.id, settings.trello.customFields.origin, { idValue: origin});
        await updateCustomField(card.id, settings.trello.customFields.windowStart, { value: { date: (new Date()).toISOString() }});

        return { status: 200 };
    }
    catch (error)
    {
        logException(error);
        return {status: 500 };
    }
}

// cards that are archived do not show in the trello UI, but do appear in searches. this function deletes every card that is currently archived so that the searches return
// only non-archived cards.

async function deleteArchivedCards()
{
    const route = `https://api.trello.com/1/boards/${settings.trello.board}/cards/closed/?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`;

    try {
        const body = await got(route, { headers: { "Accept": "application/json" }}).json();
        
        logMessage(`Deleting ${body.length} closed cards`);

        for (let i = 0; i < body.length; i++) {
            try {
                let deleteResponse = await got.delete(`https://api.trello.com/1/cards/${body[i].id}?key=${settings.trello.apiKey}&token=${settings.trello.apiToken}`, 
                    {
                        headers: { "Accept": "application/json" }
                    }
                );
            }
            catch (error)
            {
                return { status: error.response.statusCode };
            }
        }

       return { status: 200 };
    }
    catch (error)
    {
        logException(error);
        return {status: 500 };
    }
}

// a promise to lookup the station in geooptix, which has information about the well that we want to include in the trello card

async function getStationInformation(program, station) {

    let station_route = `https://${program}.api.geooptix.com/stations/${station}`;

    try {
        const station = await got(station_route, {headers: {
            "Accept": "application/json",
            "x-geooptix-token": settings.geooptix.apiKey
        }}).json();
        
        return { status: 200, body: station };
    }
    catch (error)
    {
        if (error.response.statusCode != 404)
            logException(error);

        return {status: 500 };
    }
}

async function getWellInformation(program, project, well) {

    let well_route = `https://${program}.api.geooptix.com/projects/${project}/sites/${well}`;

    try {
        const well = await got(well_route, {headers: {
            "Accept": "application/json",
            "x-geooptix-token": settings.geooptix.apiKey
        }}).json();
        
        return { status: 200, body: well };
    }
    catch (error)
    {
        if (error.response.statusCode != 404)
            logException(error);

        return {status: 500 };
    }
}

function logException(error) {
    
    if (typeof error === 'object' && error !== null)
    {
        if (requestContext)
            requestContext.log(JSON.stringify(error));
        else
            console.error(JSON.stringify(error));
    }
    else {
        if (requestContext) 
            requestContext.log(error);
        else
            console.error(error);
    }

    appInsights.defaultClient.trackException({exception: error});
}

function logMessage(message) {

    if (typeof message === 'object' && message !== null)
    {
        if (requestContext)
            requestContext.log(JSON.stringify(message));
        else
            console.info(JSON.stringify(message));
    }
    else {
        if (requestContext) 
            requestContext.log(message);
        else
            console.info(message);
    }
}

function getEventOrigin(event) {
    
    let origin = null;

    // determine orgin of the error
    if (event.eventType == "GeoOptix.Functions.PaigeWirelessGateway")
    {
        // it's either paige or geooptix
        if (event.data.status.messageCode == "UnknownEventType") 
            origin = settings.trello.origins.paige;
        else
            origin = settings.trello.origins.geooptix;
    }
    
    return origin;
}

function getSensorType(event) {
    let sensorType = null;

    if (event.data.properties.sensorType == "Paige.Message.PumpMonitor")
        sensorType = settings.trello.sensorTypes.pumpMonitor;
    else if (event.data.properties.sensorType == "Paige.Message.FlowMeter")
        sensorType = settings.trello.sensorTypes.flowMeter;
    else if (event.data.properties.sensorType == "Paige.Message.WellPressure")
        sensorType = settings.trello.sensorTypes.depthSensor;

    return sensorType;
}

// this function handles events with a status = 2 (error condition)

async function handleErrorEvent(event, origin) {

    // clean up an archived cards so that our search logic works as it should
    let response = await deleteArchivedCards();

    // what kind of a sensor are we talking about?
    let sensorType = getSensorType(event);
    
    if (!sensorType)
    {
        logException(`Could not determine the sensor type for event id ${event.id}`);
        return "Failed";
    }

    // grab the station id from the subject for lookup in GeoOpitx
    let station = event.subject.substring(event.subject.lastIndexOf("/") + 1);

    // get well information from GeoOptix
    let stationInfo = await getStationInformation(event.data.properties.tenantID, station);

    let wellNumber = null;
    let serialNumber = station;
    let location = null;
    let project = null;

    if (stationInfo.status == 200) {
        wellNumber = stationInfo.body.SiteCanonicalName;
        serialNumber = stationInfo.body.Name;
        project = stationInfo.body.ProjectCanonicalName;
    }
    
    let name = wellNumber || serialNumber;

    // see if a card for this well exists in trello yet from the same origin 
    let result = await findCardByNameAndOrigin(settings.trello.board, name, origin);
    
    if (!result.card) {
        // nope, create a new card

        logMessage(`Creating a new card for ${name}`);

        // retrieve the well information from GeoOptix to find the location

        let wellInfo = await getWellInformation(event.data.properties.tenantID, project, wellNumber);

        if (wellInfo.status == 200) {
            let coordinates = wellInfo.body.Location.geometry.coordinates;
            if (coordinates) {
                location = {
                    latitude: coordinates[1],
                    longitude: coordinates[0]
                }
            }
        }

        let createResult = createUnassignedCard 
        (
            wellNumber, 
            serialNumber,
            location, 
            sensorType, 
            event.data.status.message,
            origin
        );
    }
    else {
        // card already exists for this origin, increment the error count
        let errorCount = parseInt(result.card.errorCount) + 1;
        logMessage(`Incrementing the error count for card ${name} to ${errorCount}`);
        await updateCustomField(result.card.id, settings.trello.customFields.errorCount, { value: { number: errorCount.toString() }});
    }

    return "OK, error event handled";
}

// This is the entry point for this module 

exports.handleStatusEvent = async function(context, event) {

    requestContext = context;
    
    if (Object.keys(event.data).length === 0) {
        logException('Event data object cannot be empty');
        return 'Failed'
    }

    // try to classify the origin of this status event
    let origin = getEventOrigin(event);

    // generate origin metrics for app insights
    switch (origin)
    {
        case settings.trello.origins.geooptix:
            appInsights.trackEvent('EventOriginGeoOptix'); 
            break;
    
        case settings.trello.origins.paige:
            appInsights.defaultClient.trackEvent({name: 'EventOriginPaige'}); 
            break;

        case settings.trello.origins.aghub:
            appInsights.trackEvent('EventOriginAgHub'); 
            break;

        case settings.trello.origins.portal:
            appInsights.trackEvent('EventOriginGroundwaterPortal'); 
            break;

        case settings.trello.origins.email:
            appInsights.trackEvent('EventOriginEmail'); 
            break;

        case settings.trello.origins.user:
            appInsights.trackEvent('EventOriginUser'); 
            break;
                                        
        default:
        {
            logException(`Could not determine the origin for event id ${event.id}`);
            return "Failed";
        }
    }

    // at some point, we should create app insights metrics for Status.MessageCode to get
    // some visibility on the types of messages being received.

    switch (event.data.status.status)
    {
        case STATUS_OK:
            appInsights.trackEvent('DeviceStatusOK'); 
            return "OK, no Trello action taken";
            break;

        case STATUS_WARNING:
            appInsights.trackEvent('DeviceStatusError'); 
            return "OK, no action taken";
            break;

        case STATUS_ERROR:
            appInsights.defaultClient.trackEvent('DeviceStatusError'); 
            return await handleErrorEvent(event, origin);
            break;
    }
 
    logException(`Unrecognized device status code ${event.status.status}`);

    return "Failed";
};

(async () => {

    // overwrite settings in the config.json module with contents of environment variables if they exist. never commit actual 
    // sensitive strings in repos. the contents of the environment variables come from the following locations:
    //
    // 1. in local dev mode using VsCode from the local.settings.json file (which is excluded via .gitignore)
    // 2. in a deployed app function from the "Configuration" pane on the App Function in Azure

    if (process.env.APPINSIGHTS_INSTRUMENTATIONKEY)
    {
        settings.azure.appInsightsKey = process.env.APPINSIGHTS_INSTRUMENTATIONKEY;
    }

    console.log(`App Insights Key = ${settings.azure.appInsightsKey}`);

    // start application insights telemetry
    appInsights.setup(settings.azure.appInsightsKey)
        .setInternalLogging(settings.azure.appInsightsDeveloperMode, true)
        .start();

    if (process.env.GEOOPTIX_API_KEY) {
        settings.geooptix.apiKey = process.env.GEOOPTIX_API_KEY;
    }

    console.log(`GeoOptix API Key = ${settings.geooptix.apiKey.substring(1, 30)}...`);

    if (process.env.TRELLO_API_KEY) {
        settings.trello.apiKey = process.env.TRELLO_API_KEY;
    }

    console.log(`Trello API Key = ${settings.trello.apiKey.substring(1, 30)}...`);

    if (process.env.TRELLO_API_TOKEN) {
        settings.trello.apiToken = process.env.TRELLO_API_TOKEN;
    }

    console.log(`Trello API Token = ${settings.trello.apiToken.substring(1, 30)}...`);
})();


/*
    uncomment this block to discover the trello ids, of boards, lists, fields, and options to 
    populate the config.json file.

;
(async () => {
    console.log('Printing IDs from Trello...');

    console.log('Board details:')
    await printBoard('Device Ticket Management');

    console.log('Lists details:');
    await printLists(settings.trello.board);
    
    console.log('Custom field details:');
    await printCustomFieldIds(settings.trello.board);

    console.log('Custom field options:');
    await printCustomFieldOptions(settings.trello.customFields.origin);

})();

*/