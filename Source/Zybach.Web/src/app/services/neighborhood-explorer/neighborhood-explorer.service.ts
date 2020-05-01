import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services';
import { Observable, of } from 'rxjs';
import { FeatureCollection } from 'geojson';
import {TwinPlatteGeoJson} from './twinplatte';

@Injectable({
    providedIn: 'root'
})
export class NeighborhoodExplorerService {
    constructor(private apiService: ApiService) { }

    getMask(): Observable<object> {
                return of(TwinPlatteGeoJson);
    }
}
