import * as express from 'express'
import { UserDto } from './dtos/user-dto';

export interface RequestWithUserContext extends express.Request{
    user?: UserDto;
    auth?: {sub: string};
}