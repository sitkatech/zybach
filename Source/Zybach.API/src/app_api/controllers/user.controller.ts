import { inject } from "inversify";
import { Body, Controller, Post, Route, Security, Get, Path, Hidden, Put } from "tsoa";
import { provideSingleton } from "../../util/provide-singleton";
import { UserCreateDto, UserEditDto } from "../dtos/user-create-dto";
import { UserDto } from "../dtos/user-dto";
import { RoleEnum } from "../models/role";
import { SecurityType } from "../security/authentication";
import { UserService } from "../services/user-service";


@Route("/api/users")
@Hidden()
@provideSingleton(UserController)
export class UserController extends Controller{
    constructor(@inject(UserService) private userService: UserService){
        super();
    }

    @Get("")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async list() : Promise<UserDto[]>{
        return await this.userService.list()
    }
    
    @Post("")
    @Security(SecurityType.ANONYMOUS)
    public async createUser(
        @Body() user: UserCreateDto
    ): Promise<UserDto> {
        const newUser = await this.userService.addUser(user);
        return newUser;
    }

    @Get("unassigned-report")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getUnassignedUsers() : Promise<{Count: number}>{
        const countOfUnassignedUsers = await this.userService.getCountOfUnassignedUsers();
        return {Count: countOfUnassignedUsers};
    }

    @Put("set-disclaimer-acknowledged-date")
    //todo: no way this should actually be anonymous
    @Security(SecurityType.ANONYMOUS)
    public async setDisclaimerAcknowledgedDate(
        @Body() body: {UserGuid: string}
    ) {
        return await this.userService.setDisclaimerAcknowledgedDate(body.UserGuid);
    }

    @Get("{userID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async getByID(
        @Path() userID: string
    ): Promise<UserDto> {
        return await this.userService.getUserById(userID);
    }

    @Put("{userID}")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async updateUser(
        @Path() userID: string,
        @Body() userEditDto: UserEditDto

    ): Promise<UserDto> {
        return await this.userService.updateUser(userID, userEditDto);
    }

    @Get("user-claims/{guid}")
    @Security(SecurityType.ANONYMOUS)
    public async getByClaim(
        @Path() guid: string
    ) : Promise<UserDto>{
        const user = await this.userService.getUserByGuid(guid);
        return user;
    }
}