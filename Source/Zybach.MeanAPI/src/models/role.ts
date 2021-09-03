import {EnumToArray} from '../util';
import { RoleDto } from '../dtos/role-dto';

export enum RoleEnum{
    Adminstrator = "Administrator",
    Disabled = "Disabled",
    Unassigned = "Unassigned"
}

export const RoleDBOptions = EnumToArray(RoleEnum);

const LegacyRoleIDMap = {
    "Administrator": 1,
    "Disabled" : 2,
    "Unassigned": 3
}

export function GetLegacyRole(role: RoleEnum): RoleDto{
    return {
        RoleDisplayName: role,
        RoleName: role,
        RoleID: LegacyRoleIDMap[role]
    };
}