import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SiteDto } from '../shared/models/geooptix/site-dto';
import { environment } from 'src/environments/environment';
import { map, filter, switchMap } from 'rxjs/operators';
import { throwIfNoContent } from '../shared/static-functions';


@Injectable({
  providedIn: 'root'
})
export class WellService {
  getTimeSeriesData(wellCanonicalName: string, sensorCanonicalName: any, folderCanonicalName: any): Observable<any> {
    const route = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders/${folderCanonicalName}/files/data.json/download`
    return this.httpClient.get(route, {observe:"response", responseType:"text"}).pipe(map(x=>{
      return JSON.parse('{"data": [' + x.body.slice(0, -1) + ']}');
    }));
  }
  getFiles(wellCanonicalName: string, sensorCanonicalName: any, folderCanonicalName: any): Observable<any> {
    const route = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders/${folderCanonicalName}/files`

    return this.httpClient.get<any>(route);
  }

  constructor(private httpClient: HttpClient) { }

  public getSites(): Observable<SiteDto[]> {
    const route = `${environment.geooptixHostName}/project-overview-web/water-data-program/sites`;

    return this.httpClient.get<SiteDto[]>(route);
  }

  public getSite(canonicalName: string): Observable<SiteDto> {
    const route = `${environment.geooptixHostName}/project-overview-web/water-data-program/sites/${canonicalName}`;

    return this.httpClient.get<SiteDto>(route);
  }

  public getSensorName(canonicalName: string): Observable<any>{
    const initialRoute = `${environment.geooptixHostName}/projects/water-data-program/sites/${canonicalName}/stations`;
    return this.httpClient.get<any>(initialRoute);//.pipe(map(x=>x.CanonicalName))
  }

  public getSensorFolder(wellCanonicalName: string, sensorCanonicalName: string): Observable<any> {
    const initialRoute = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders`;
    return this.httpClient.get<any>(initialRoute);
  }
}
