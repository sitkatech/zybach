import {UserDto} from '../generated/user-dto'

export class UserDetailedDto extends UserDto {
    FullName : string;

    constructor(obj?: any) {
        super()
        Object.assign(this, obj);
    }
}

