import { Controller, Route, Get, Hidden } from "tsoa";
import { RoleDto } from "../dtos/role-dto";
import { GetLegacyRole, RoleEnum } from "../models/role";


@Route("/api/roles")
@Hidden()
export class RoleController extends Controller{
    @Get("")
    public async getRoles() : Promise<RoleDto[]> {
        return [
            GetLegacyRole(RoleEnum.Adminstrator),
            GetLegacyRole(RoleEnum.Unassigned)
        ];
    }
}