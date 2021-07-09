import { ApiError } from "./apiError";

export class NotFoundError extends ApiError {
    constructor(message?: string) {
        super('Not Found', 404, message);
    }
}