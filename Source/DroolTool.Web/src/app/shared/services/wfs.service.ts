import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {FeatureCollection} from "geojson";
import {Observable} from "rxjs";
import {environment} from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class WfsService {

    constructor(
        private http: HttpClient,
    ) {
    }

    public geoserverNeighborhoodLookup(latlng: Object): Observable<FeatureCollection> {
        let x1 = latlng['lng'];
        let y1 = latlng['lat'];
        let x2 = x1 + 0.0001;
        let y2 = y1 + 0.0001;

        var bbox = [x1, y1, x2, y2].join(",");

        console.log(bbox);

        const url: string = `${environment.geoserverMapServiceUrl}/wms`;
        return this.http.get<FeatureCollection>(url, {
            params: {
                service: 'WMS',
                version: '1.1.1',
                request: "GetFeatureInfo",
                info_format: "application/json",
                QUERY_LAYERS: 'DroolTool:Neighborhoods',
                layers: 'DroolTool:Neighborhoods',
                x: '50',
                y: '50',
                SRS: 'EPSG:4326',
                width: '101',
                height: '101',
                bbox: bbox
            }
        })
    }
}
