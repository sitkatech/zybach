import secrets from '../../secrets';
import { UserDto } from '../dtos/user-dto';
import { UnauthorizedError } from '../../errors/unauthorized-error';
import { ApiError } from '../../errors/apiError';
import { UserService } from '../services/user-service';
import { RequestWithUserContext } from '../request-with-user-context';

export enum SecurityType {
    API_KEY = "api_key",
    KEYSTONE = "keystone",
    ANONYMOUS = "anonymous"
}

export async function expressAuthentication(req: RequestWithUserContext, securityName: string, scopes?: string[]): Promise<UserDto> {
    // included to keep vscode from removing the scopes declaration if you do ctrl+. "Remove all unused declarations"
    // can be removed once we start using this to define allowed roles on routes
    console.log(scopes);
    
    // if endpoint is API-key authenticated, don't bother getting the user context
    if (securityName === SecurityType.API_KEY) {
        let keyValue = secrets.API_KEY_VALUE;
        let keySent = req.get('x-api-authorization');
        if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
            return Promise.reject(new UnauthorizedError("Missing or invalid API key"));
        } else {
            return Promise.resolve({ IsAnonymous: true });
        }
    }

    if (securityName === SecurityType.ANONYMOUS){
        return Promise.resolve({IsAnonymous: true});
    }
    else if (securityName === SecurityType.KEYSTONE) {
        if (!req.auth){
            throw new UnauthorizedError("Authorization missing from header.");
        }

        const user = await new UserService().getUserByGuid(req.auth.sub);

        return Promise.resolve(user);
    }

    else {
        return Promise.reject(new ApiError("Internal Server Error", 500, `Unsupported authentication ('${securityName}') type on requested resource.`));
    }
};
