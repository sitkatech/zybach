export interface ApiResult<T>{
    status: string;
    result: T;
}

export interface ErrorResult {
    status: string;
    message: string;
} 