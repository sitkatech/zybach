import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services';
import { Observable } from 'rxjs';
import { FeatureCollection } from 'geojson';

@Injectable({
    providedIn: 'root'
})
export class NeighborhoodExplorerService {
    constructor(private apiService: ApiService) { }

    getMask(): Observable<string> {
        let route = `/neighborhood-explorer/get-mask`;
        return this.apiService.getFromApi(route);
    }

    getStormshed(neighborhoodID:number): Observable<string> {
        let route = `/neighborhood-explorer/get-stormshed/${neighborhoodID}`;
        return this.apiService.getFromApi(route);
    }

    getDownstreamBackboneTrace(neighborhoodID:number): Observable<string> {
        let route = `/neighborhood-explorer/get-downstream-backbone-trace/${neighborhoodID}`;
        return this.apiService.getFromApi(route);
    }

    getServicedNeighborhoodIds(): Observable<number[]> {
        let route = `/neighborhood-explorer/get-serviced-neighborhood-ids`;
        return this.apiService.getFromApi(route);
    }
    
}
