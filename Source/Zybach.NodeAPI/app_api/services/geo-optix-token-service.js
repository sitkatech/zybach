const { poolPromise } = require('../../db')
const sql = require('mssql');
const axios = require('axios');
const querystring = require('querystring');
const secrets = require('../../secrets');

const getGeoOptixAccessToken = async function () {
    return new Promise(async function (resolve, reject) {
        try {
            const pool = await poolPromise
            const getAccessTokenResult = await pool.request()
                .query('select top 1 * from dbo.GeoOptixAccessToken');
            let currentToken = getAccessTokenResult.recordset.length > 0 ? getAccessTokenResult.recordset[0] : null;
            if (currentToken == null || ((new Date(currentToken.GeoOptixAccessTokenExpiryDate).getTime() - new Date().getTime()) / 1000 * 60 * 60) < 2) {
                try {
                    const newTokenRequest = await makeKeystoneTokenRequest();
                    if (currentToken != null) {
                        await deleteTableRecords();
                    }
                    const newToken = {
                        GeoOptixAccessTokenValue: newTokenRequest.access_token,
                        GeoOptixAccessTokenExpiryDate: new Date(new Date().getTime() + newTokenRequest.expires_in * 1000)
                    };
                    await insertNewTokenIntoDatabase(newToken);
                    currentToken = newToken;
                }
                catch (err) {
                    reject(err);
                }
            }
            resolve(currentToken.GeoOptixAccessTokenValue);
        } catch (err) {
            console.log(err.message);
            reject(err.message);
        }
    });
}

async function deleteTableRecords() {
    return new Promise(async function (resolve, reject) {
        try {
            const pool = await poolPromise
            const result = await pool.request()
                .query('delete from dbo.GeoOptixAccessToken');
            resolve(result);
        }
        catch (err) {
            reject(err);
        }
    });
}

async function insertNewTokenIntoDatabase(newTokenObject) {
    return new Promise(async function (resolve, reject) {
        try {
            const pool = await poolPromise
            const result = await pool.request()
                .input('newToken', sql.VarChar(2048), newTokenObject.GeoOptixAccessTokenValue)
                .input('expiryDate', sql.DateTime, newTokenObject.GeoOptixAccessTokenExpiryDate)
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
        try {
            const keystoneTokenRequest = await axios.post(secrets.KEYSTONE_AUTHORITY_URL,
                querystring.stringify({
                    client_id: secrets.ZYBACH_CLIENT_ID,
                    client_secret: secrets.ZYBACH_CLIENT_SECRET,
                    scope: "openid all_claims keystone",
                    grant_type: "password",
                    username: secrets.GEOOPTIX_USERNAME,
                    password: secrets.GEOOPTIX_PASSWORD
                }), {
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                }
            })
            resolve(keystoneTokenRequest.data);
        }
        catch (err) {
            reject(err);
        }
    });
}

module.exports = { getGeoOptixAccessToken }