import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class NeighborhoodExplorerService {
    constructor(private apiService: ApiService) { }

    getMask(): Observable<string> {
        let route = `/neighborhood-explorer/get-mask`;
        return this.apiService.getFromApi(route);
    }
    
}
