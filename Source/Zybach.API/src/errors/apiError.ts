export class ApiError extends Error{
    public status: number;
    constructor (name: string, statusCode: number, message?: string){
        super(message);
        this.name = name;
        this.status = statusCode;
    }
}
