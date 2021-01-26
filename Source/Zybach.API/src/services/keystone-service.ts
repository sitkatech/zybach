import { provideSingleton } from "../util/provide-singleton";
import got from 'got';
import { KeystoneInviteDto } from "../dtos/keystone-invite-dto";
import secrets from "../secrets";
import { InternalServerError } from "../errors/internal-server-error";

@provideSingleton(KeystoneService)
export class KeystoneService {
    public async invite(inviteDto:KeystoneInviteDto, token: string): Promise<any>{
        let result;
        try {
        const inviteUrl = `${process.env["KEYSTONE_BASE_URL"]}/api/v1/invite`
        result = await got.post(inviteUrl, {
            headers: {
                Authorization: token
            },
            json: inviteDto,
            responseType: "json",
        });
        } catch (err){
            console.error(err)
            throw new InternalServerError("Error submitting user to Keystone");
        }

        if (result.statusCode !== 200){
            console.error(`Invite User Error. Keystone returned ${result.statusCode}`);
            throw new InternalServerError("Error submitting user to Keystone");
        }

        return result.body;
    }
}
