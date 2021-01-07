import secrets from '../../secrets';
import * as express from 'express';
import { UserDto } from '../dtos/user-dto';
import { UnauthorizedError } from '../../errors/unauthorized-error';
import { ApiError } from '../../errors/apiError';
import { promises } from 'fs';
import { UserService } from '../services/user-service';

export enum SecurityType {
    API_KEY = "api_key",
    KEYSTONE = "keystone",
    ANONYMOUS = "anonymous"
}

export function expressAuthentication(req: express.Request, securityName: string, scopes?: string[]): Promise<UserDto> {
    // if endpoint is API-key authenticated, don't bother getting the user context
    if (securityName === SecurityType.API_KEY) {
        let keyValue = secrets.API_KEY_VALUE;
        let keySent = req.get('authorization');
        if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
            return Promise.reject(new UnauthorizedError("Missing or invalid API key"));
        } else {
            return Promise.resolve({ UserName: "Api User", UserID: -1 });
        }
    }

    var user = new UserService().getUser(req);

    if (securityName === SecurityType.ANONYMOUS){
        return Promise.resolve(user);
    }
    else if (securityName === SecurityType.KEYSTONE) {
        
        return Promise.resolve(user);
    } else {
        return Promise.reject(new ApiError("Internal Server Error", 500, `Unsupported authentication ('${securityName}') type on requested resource.`));
    }
};
