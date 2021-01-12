import { Controller, Route, Get, Hidden } from "tsoa";
import { RoleDto } from "../dtos/user-dto";


@Route("/api/roles")
@Hidden()
export class RoleController extends Controller{
    @Get("")
    public async getRoles() : Promise<RoleDto[]> {
        return [
            {
                RoleID: 1,
                RoleDisplayName: "Administrator"
            },
            {
                RoleID:3,
                RoleDisplayName:"Unassigned"
            }
        ]
    }
}