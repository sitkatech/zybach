import User, { UserInterface } from "../models/user";

export class UserDto {
    UserID?: string;
    UserGuid?: string;
    FirstName?: string;
    LastName?: string;
    FullName?: string;
    Email?: string;
    Phone?: string;
    CreateDate?: Date;
    UpdateDate?: Date;
    LastActivityDate?: Date;
    DisclaimerAcknowledgedDate?: Date;
    ReceiveSupportEmails?: boolean;
    LoginName?: string;
    Company?: string;
    IsAnonymous?: boolean;
    Role?: RoleDto;
  }

  export class UserDtoFactory{
      public static FromModel(user: UserInterface) : UserDto{
          return {
              UserID: user._id.toString(),
              UserGuid: user.UserGuid,
              FirstName: user.FirstName,
              LastName: user.LastName,
              FullName: `${user.FirstName} ${user.LastName}`,
              Email: user.Email,
              Phone: user.Phone,
              CreateDate: user.CreateDate,
              UpdateDate: user.UpdateDate,
              LastActivityDate: user.LastActivityDate,
              DisclaimerAcknowledgedDate: user.DisclaimerAcknowledgedDate,
              ReceiveSupportEmails: user.ReceiveSupportEmails,
              LoginName: user.LoginName,
              Company: user.Company,
              // todo: currently hardcoding role to sidestep the question of how to represent roles in the database
              Role: {
                  RoleID: 1,
                  RoleDisplayName: "Administrator"
              }
          }
      }
  }

  export interface RoleDto{
      RoleID: number;
      RoleDisplayName: string;
  }