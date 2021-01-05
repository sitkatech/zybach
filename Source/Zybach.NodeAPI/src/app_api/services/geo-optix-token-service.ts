import axios from 'axios';
import querystring from 'querystring';
import secrets from '../../secrets'
import GeoOptixToken, { GeoOptixTokenInterface } from '../models/geo-optix-token';
import { ApiError } from '../../errors/apiError';
import { GeoOptixTokenDto } from '../dtos/geo-optix-token-dto';

export class GeoOptixTokenService {
    
    public async getGeoOptixAccessToken() {
        var self = this;
        let currentToken: GeoOptixTokenInterface | null;

        try {
            currentToken = await GeoOptixToken.findOne();
            //let currentToken = getAccessTokenResult.recordset.length > 0 ? getAccessTokenResult.recordset[0] : null;
            if (currentToken == null || ((new Date(currentToken.ExpirationDate).getTime() - new Date().getTime()) / 1000 * 60 * 60) < 2) {

                const newTokenRequest = await self.makeKeystoneTokenRequest();
                if (currentToken != null) {
                    await self.clearExistingTokens();
                }
                const newToken: GeoOptixTokenDto = {
                    TokenValue: newTokenRequest.access_token as string,
                    ExpirationDate: new Date(new Date().getTime() + newTokenRequest.expires_in * 1000)
                };
                currentToken = await self.insertToken(newToken);
            }
        }
        catch (err){
            throw new ApiError("Internal Server Error", 500, err.message);
        }

        return currentToken.TokenValue;
    }

    async clearExistingTokens() {
        try {
            await GeoOptixToken.deleteMany();
        } catch (err) {
            throw new ApiError("Internal Server Error", 500, "Error deleting old GeoOptix tokens");
        }
    }

    async insertToken(newTokenObject: GeoOptixTokenDto): Promise<GeoOptixTokenInterface> {
        const newToken = new GeoOptixToken(newTokenObject);

        try {
            await newToken.save();
        }
        catch (err) {
            throw new ApiError("Internal Server Error", 500, "Error inserting new GeoOptix token");
        }

        return newToken;
    }

    async makeKeystoneTokenRequest(): Promise<{ access_token: any, expires_in: any }> {
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
}
