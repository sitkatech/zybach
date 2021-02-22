export interface UserCreateDto{
    LoginName?: string;
    UserGuid?: string;
    FirstName: string;
    LastName: string;
    Email: string;
    Role?: string;
}

export interface UserEditDto{
    Role?: string;
    ReceiveSupportEmails?: boolean
    UserGuid?: string;
}