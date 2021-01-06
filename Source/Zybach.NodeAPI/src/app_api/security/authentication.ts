import secrets from '../../secrets';
import * as express from 'express';
import { UserDto } from '../dtos/user-dto';
import { UnauthorizedError } from '../../errors/unauthorized-error';
import { ApiError } from '../../errors/apiError';

export function expressAuthentication(req: express.Request, securityName: string, scopes?: string[]): Promise<UserDto> {
    if (securityName === "key") {
        let keyValue = secrets.API_KEY_VALUE;
        let keySent = req.get('authorization');
        if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
            return Promise.reject(new UnauthorizedError("Missing or invalid API key"));
        } else {

            return Promise.resolve({ UserName: "Api User", UserID: -1 });
        }

    } else if (securityName === "feature") {
        if (scopes === undefined) {
            return Promise.reject(new ApiError("Internal Server Error", 500, "No feature scopes provided on requested resource."));
        }

        // todo: get user 

        scopes.forEach(scope => {
            console.log(`Authenticated for ${scope}`)
        });

        return Promise.resolve({UserName: "Fred Barnacles", UserID: -2});
    } else {
        return Promise.reject(new ApiError("Internal Server Error", 500, `Unsupported authentication ('${securityName}') type on requested resource.`));
    }
};
