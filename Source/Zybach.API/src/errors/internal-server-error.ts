import { ApiError } from "./apiError";

export class InternalServerError extends ApiError {
    constructor(message: string) {
        super("Internal Server Error", 500, message);
    }
}
