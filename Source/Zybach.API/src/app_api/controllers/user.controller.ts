import { Body, Controller, Post, Route, Security, Get, Path, Hidden, Put } from "tsoa";
import { UserCreateDto, UserEditDto } from "../dtos/user-create-dto";
import { UserDto } from "../dtos/user-dto";
import { RoleDBOptions, RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { UserService } from "../services/user-service";


@Route("/api/users")
@Hidden()
export class UserController extends Controller{
    @Get("")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async list() : Promise<UserDto[]>{
        return await new UserService().list()
    }
    
    @Post("")
    @Security(SecurityType.ANONYMOUS)
    public async createUser(
        @Body() user: UserCreateDto
    ): Promise<UserDto> {
        const newUser = await new UserService().addUser(user);
        return newUser;
    }

    @Get("unassigned-report")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getUnassignedUsers() : Promise<{Count: number}>{
        const countOfUnassignedUsers = await new UserService().getCountOfUnassignedUsers();
        return {Count: countOfUnassignedUsers};
    }

    @Put("set-disclaimer-acknowledged-date")
    //todo: no way this should actually be anonymous
    @Security(SecurityType.ANONYMOUS)
    public async setDisclaimerAcknowledgedDate(
        @Body() body: {UserGuid: string}
    ) {
        return await new UserService().setDisclaimerAcknowledgedDate(body.UserGuid);
    }

    @Get("{userID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getByID(
        @Path() userID: string
    ): Promise<UserDto> {
        console.log(RoleDBOptions);
        return await new UserService().getUserById(userID);
    }

    @Put("{userID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async updateUser(
        @Path() userID: string,
        @Body() userEditDto: UserEditDto

    ): Promise<UserDto> {
        return await new UserService().updateUser(userID, userEditDto);
    }

    @Get("user-claims/{guid}")
    @Security(SecurityType.ANONYMOUS)
    public async getByClaim(
        @Path() guid: string
    ) : Promise<UserDto>{
        const user = await new UserService().getUserByGuid(guid);
        return user;
    }
}