import { UserDto, UserDtoFactory } from "../dtos/user-dto";
import User, { UserInterface } from "../models/user";
import { NotFoundError } from "../../errors/not-found-error";
import { UserCreateDto } from "../dtos/user-create-dto";
import { ApiError } from "../../errors/apiError";


export class UserService{
    public async getCountOfUnassignedUsers() : Promise<number> {
        // todo: implement
        return 0;
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
        } catch{
            throw new ApiError("Internal Server Error", 500, "Failed to fetch users")
        }
    }

    public async setDisclaimerAcknowledgedDate(userGuid: string): Promise<UserDto> {
        let updatedUser;
        try{
            updatedUser = await User.findOneAndUpdate({UserGuid: userGuid}, {DisclaimerAcknowledgedDate: new Date()}, {new: true})
        }
        catch{
            throw new ApiError("Internal Server Error", 500, "Failed to udate disclaimer date")
        }
        
        if (updatedUser === null){
            throw new NotFoundError("User Not Found");
        }

        return updatedUser
    }

    public async addUser(user: UserCreateDto): Promise<UserDto> {

        const xyz = user as UserDto;

        xyz.CreateDate = new Date();
        xyz.ReceiveSupportEmails = false;

        const newUser = new User(user);

        try {
            await newUser.save();
         } catch (err){
             throw new ApiError("Internal Server Error", 500, "Adding user failed");
         }

        return newUser;
    }

    public async getUserByGuid(guid: string) : Promise<UserDto>{
        const user = await User.findOne({UserGuid: guid})

        if (user === null){
            throw new NotFoundError("User Not Found");
        }

        return UserDtoFactory.FromModel(user);
    }
}
