import secrets from '../secrets';
import { UserDto } from '../dtos/user-dto';
import { UnauthorizedError } from '../errors/unauthorized-error';
import { ApiError } from '../errors/apiError';
import { UserService } from '../services/user-service';
import { RequestWithUserContext } from '../request-with-user-context';
import { promises } from 'fs';
import { InternalServerError } from '../errors/internal-server-error';

export enum SecurityType {
    API_KEY = "api_key",
    KEYSTONE = "keystone",
    ANONYMOUS = "anonymous"
}

export async function expressAuthentication(req: RequestWithUserContext, securityName: string, scopes?: string[]): Promise<UserDto> {
    // if endpoint is API-key authenticated, don't bother getting the user context
    if (securityName === SecurityType.API_KEY) {
        let keyValue = secrets.API_KEY_VALUE;
        let keySent = req.get('x-api-authorization');
        if (keySent == null || keySent == undefined || keyValue === null || keyValue === undefined || keySent !== keyValue) {
            if (req.auth) {
                // fall back to Keystone authentication
                securityName = SecurityType.KEYSTONE;
            } else {
                // since keystone auth not present, reject access
                return Promise.reject(new UnauthorizedError("Missing or invalid API key"));
            }
        } else {
            return Promise.resolve({ IsAnonymous: true });
        }
    }

    if (securityName === SecurityType.ANONYMOUS) {
        return Promise.resolve({ IsAnonymous: true });
    }
    else if (securityName === SecurityType.KEYSTONE) {
        if (!req.auth) {
            throw new UnauthorizedError("Authorization missing from header.");
        }
        const user = await new UserService().getUserByGuid(req.auth.sub)

        if (!scopes || !scopes.length) {
            return Promise.resolve(user);
        }

        for (var i = 0; i < scopes.length; i++) {
            if (user.Role?.RoleName === scopes[i]) {
                return Promise.resolve(user);
            }
        }

        throw new UnauthorizedError("User does not have the necessary role to complete this action");
    }

    else {
        return Promise.reject(new InternalServerError(`Unsupported authentication ('${securityName}') type on requested resource.`));
    }
};