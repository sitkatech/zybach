import { UserDto } from "../dtos/user-dto";
import * as express from 'express';

export class UserService{
    public getUser(req: express.Request) : UserDto {
        return {UserName: "FrankZybach", UserID: -2};
    }
}
