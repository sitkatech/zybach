import { HttpResponse } from '@angular/common/http';

export function throwIfNoContent<T>(response: HttpResponse<T>): T {

    if (response.status != 200) {
        throw new Error(response.status + ' No Content');
    } // If everything went fine, return the response 
    else {
        return response.body;
    }
}