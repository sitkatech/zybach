export interface UserCreateDto{
    LoginName: string;
    UserGuid: string;
    FirstName: string;
    LastName: string;
    Email: string;
}

export interface UserEditDto{
    Role: string;
    ReceiveSupportEmails: boolean
}