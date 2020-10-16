//const config = require("./config");
const baseUrl = "https://i7mkwlfa1b.execute-api.us-east-2.amazonaws.com/prod";
const { Client } = require('node-rest-client');
const config = require('../config');

const client = new Client();
const args = { data: "", headers: { "x-api-key": config.zappaApiKey } };

function getPumpingRate(wellRegistrationID) {
    return new Promise((resolve, reject) => {
        url = `${baseUrl}/wells/${wellRegistrationID.toUpperCase()}/summaryStatistics`;

        client.get(url, args, (body, response) => {
            if (response.statusCode == 200) {
                const pumpingRate = body.wellAuditPumpRate ? body.wellAuditPumpRate :
                    (body.wellRegisteredPumpRate ? body.wellRegisteredPumpRate :
                        (body.wellTpnrdPumpRate ? body.wellTpnrdPumpRate : 0)
                    );
                if (!pumpingRate) {
                    console.error(`No Pumping Rate found for Well ${wellRegistrationID}`);
                }
                resolve(pumpingRate);
            } else {
                // todo is this still right?
                console.error(`No Pumping Rate found for Well ${wellRegistrationID}`);
                resolve(0);
            }
        })
    });

}

module.exports.getPumpingRate = getPumpingRate;