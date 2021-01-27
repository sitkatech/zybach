import { Controller, Route, Get, Hidden } from "tsoa";
import { provideSingleton } from "../util/provide-singleton";
import { RoleDto } from "../dtos/role-dto";
import { GetLegacyRole, RoleEnum } from "../models/role";


@Route("/api/roles")
@Hidden()
@provideSingleton(RoleController)
export class RoleController extends Controller{
    @Get("")
    public async getRoles() : Promise<RoleDto[]> {
        return [
            GetLegacyRole(RoleEnum.Adminstrator),
            GetLegacyRole(RoleEnum.Disabled),
            GetLegacyRole(RoleEnum.Unassigned)
        ];
    }
}