import { UserDto, UserDtoFactory } from "../dtos/user-dto";
import User, { UserInterface } from "../models/user";
import { NotFoundError } from "../errors/not-found-error";
import { UserCreateDto, UserEditDto } from "../dtos/user-create-dto";
import { InternalServerError } from "../errors/internal-server-error";
import { provideSingleton } from "../util/provide-singleton";
import { RoleEnum } from "../models/role";

@provideSingleton(UserService)
export class UserService{
    public async getByEmail(Email: string): Promise<UserDto | null> {
        const user = await User.findOne({Email: Email})

        if (user === null){
            return null;
        }

        return UserDtoFactory.FromModel(user);
    }
    
    public async updateUser(userID: string, userEditDto: UserEditDto): Promise<UserDto> {
        let updatedUser;

        try {
            updatedUser = await User.findOneAndUpdate({_id : userID}, userEditDto, {new: true} );
        }
        catch (err){
            console.error(err)
            throw new InternalServerError("Failed to udate user");
        }

        return UserDtoFactory.FromModel(updatedUser);
    }
    public async getCountOfUnassignedUsers() : Promise<number> {
        try {
            return  await User.count({Role: RoleEnum.Unassigned})
        } catch {
            throw new InternalServerError("Error retrieving unassigned users");
        }
    }
    public async getUserById(userID: string): Promise<UserDto> {
        const user = await User.findOne({_id: userID})

        if (user === null){
            throw new NotFoundError("User Not Found");
        }

        return UserDtoFactory.FromModel(user);
    }
    
    public async list(): Promise<UserDto[]> {
        try {
            const userModels = await User.find();
            return userModels.map((x:UserInterface) => UserDtoFactory.FromModel(x));
        } catch (err) {
            console.error(err)
            throw new InternalServerError("Failed to fetch users")
        }
    }

    public async setDisclaimerAcknowledgedDate(userGuid: string): Promise<UserDto> {
        let updatedUser;
        try{
            updatedUser = await User.findOneAndUpdate({UserGuid: userGuid}, {DisclaimerAcknowledgedDate: new Date()}, {new: true})
        }
        catch (err) {
            console.error(err);
            throw new InternalServerError("Failed to udate disclaimer date")
        }
        
        if (updatedUser === null){
            throw new NotFoundError("User Not Found");
        }

        return updatedUser
    }

    public async addUser(user: UserCreateDto): Promise<UserDto> {

        const xyz = user as UserInterface;

        xyz.CreateDate = new Date();
        xyz.ReceiveSupportEmails = false;
        xyz.Role = user.Role || "Unassigned";

        const newUser = new User(user);

        try {
            await newUser.save();
         } catch (err){
             console.error(err)
             throw new InternalServerError("Adding user failed");
         }

        return UserDtoFactory.FromModel(newUser);
    }

    public async getUserByGuid(guid: string) : Promise<UserDto>{
        const user = await User.findOne({UserGuid: guid})

        if (user === null){
            throw new NotFoundError("User Not Found");
        }

        return UserDtoFactory.FromModel(user);
    }
}