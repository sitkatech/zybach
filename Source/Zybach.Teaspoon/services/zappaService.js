//const config = require("./config");
const { Client } = require('node-rest-client');
const config = require('../config');
const baseUrl = config.zappaBaseUrl;

const client = new Client();
const args = { data: "", headers: { "x-api-key": config.zappaApiKey } };

function getPumpingRate(wellRegistrationID) {
    return new Promise((resolve, reject) => {
        url = `${baseUrl}/wells/${wellRegistrationID.toUpperCase()}/summary-statistics`;

        client.get(url, args, (body, response) => {
            if (response.statusCode == 200) {
                const data = body.data;
                const pumpingRate = data.wellAuditPumpRate ? data.wellAuditPumpRate :
                    (data.wellRegisteredPumpRate ? data.wellRegisteredPumpRate :
                        (data.wellTpnrdPumpRate ? data.wellTpnrdPumpRate : 0)
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