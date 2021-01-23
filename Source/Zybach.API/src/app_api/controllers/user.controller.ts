import { inject } from "inversify";
import { Body, Controller, Post, Route, Security, Get, Path, Hidden, Put, Request } from "tsoa";
import secrets from "../../secrets";
import { provideSingleton } from "../../util/provide-singleton";
import { KeystoneInviteDto } from "../dtos/keystone-invite-dto";
import { UserCreateDto, UserEditDto } from "../dtos/user-create-dto";
import { UserDto } from "../dtos/user-dto";
import { RoleEnum } from "../models/role";
import { RequestWithUserContext } from "../request-with-user-context";
import { SecurityType } from "../security/authentication";
import { KeystoneService } from "../services/keystone-service";
import { UserService } from "../services/user-service";


@Route("/api/users")
@Hidden()
@provideSingleton(UserController)
export class UserController extends Controller{
    constructor(
        @inject(UserService) private userService: UserService,
        @inject(KeystoneService) private keystoneService: KeystoneService
    ){
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

    @Post("invite")
    @Security(SecurityType.KEYSTONE, [RoleEnum.Adminstrator])
    public async inviteUser(
        @Body() userCreateDto: UserCreateDto,
        @Request() req: RequestWithUserContext
    ): Promise<UserDto>
    {
        const keystoneInviteDto: KeystoneInviteDto = {
            FirstName : userCreateDto.FirstName,
            LastName: userCreateDto.LastName,
            Email: userCreateDto.Email,
            // todo: envs
            Subject: "Invitation to the TPNRD Groundwater Platform",
            WelcomeText: "You are receiving this notification because an administrator of the TPNRD Groundwater Platform, an online service of the Twin Platte Natural Resources District, has invited you to create an account.",
            SiteName: "TPNRD Groundwater Platform",
            // todo
            SignatureBlock: "",
            RedirectUrl: process.env["KEYSTONE_REDIRECT_URL"] as string
        }
        const result = await this.keystoneService.invite(keystoneInviteDto, req.headers.authorization as string)

        const keystoneUser : {email: string, userGuid:string, firstName: string, lastName: string} = result.claims

        let existingUser = await this.userService.getByEmail(keystoneUser.email);

        if (existingUser){
            existingUser = await this.userService.updateUser(existingUser.UserID as string, {UserGuid: keystoneUser.userGuid})
            return existingUser;
        }

        const newUser: UserCreateDto = {
            FirstName: keystoneUser.firstName,
            LastName: keystoneUser.lastName,
            Email: keystoneUser.email,
            Role: userCreateDto.Role,
            LoginName: keystoneUser.email,
            UserGuid: keystoneUser.userGuid
        }

        return await this.userService.addUser(newUser);
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