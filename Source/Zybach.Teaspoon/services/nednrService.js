const { Client } = require('node-rest-client');
const wellsFromNednrAPI = require('../scrappy/TwinPlatte');

const registrationIDWellIDMapping = wellsFromNednrAPI.reduce((map, obj) => {
    map[obj.RegistrationNumber] = obj.WellID;
    return map;
}, {});

client = new Client();

// todo: this function needs to expose the wells that didn't get pumping rates found in a way that can be logged/cached in a way that can be reconsumed later when we need to heal
function getGpmFromNednrAPI(wellRegistrationID) {
    return new Promise((resolve, reject) => {
        const wellID = registrationIDWellIDMapping[wellRegistrationID];

        if (!wellID) {
            console.error(`No Pumping Rate found for Well ${wellRegistrationID}`)
            resolve(0);
            return;
        }

        let endpoint = `https://nednr.nebraska.gov/IwipApi/api/v1/Wells/Well?id=${wellID}`;

        args = { data: "", headers: {} };

        client.get(endpoint, args, (body, response) => {
            if (response.statusCode == 200) {
                resolve(body.Result.PumpRate);
            } else {
                console.error(`No Pumping Rate found for Well ${wellRegistrationID}`)
                resolve(0);
            }
        })
    });
}

module.exports = getGpmFromNednrAPI;