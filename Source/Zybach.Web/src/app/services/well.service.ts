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

  constructor(private httpClient: HttpClient) { }

  public getSites(): Observable<SiteDto[]> {
    const route = `${environment.geooptixHostName}/project-overview-web/water-data-program/sites`;

    return this.httpClient.get<SiteDto[]>(route);
  }

  public getSite(canonicalName: string): Observable<SiteDto> {
    const route = `${environment.geooptixHostName}/project-overview-web/water-data-program/sites/${canonicalName}`;

    return this.httpClient.get<SiteDto>(route);
  }

  // these methods make a lot of assumptions about the data model of the TPNRD GeoOptix program.

  // todo: DTOs for these responses would be nice for type-safety, but this code works and gets us to MVP
  public getSensorName(canonicalName: string): Observable<any>{
    const initialRoute = `${environment.geooptixHostName}/projects/water-data-program/sites/${canonicalName}/stations`;
    return this.httpClient.get<any>(initialRoute);
  }

  public getSensorFolder(wellCanonicalName: string, sensorCanonicalName: string): Observable<any> {
    const initialRoute = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders`;
    return this.httpClient.get<any>(initialRoute);
  }

  public getFiles(wellCanonicalName: string, sensorCanonicalName: string, folderCanonicalName: string): Observable<any> {
    const route = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders/${folderCanonicalName}/files`

    return this.httpClient.get<any>(route);
  }

  public getTimeSeriesData(wellCanonicalName: string, sensorCanonicalName: string, folderCanonicalName: string): Observable<any> {
    const route = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders/${folderCanonicalName}/files/data.json/download`
    
    return this.httpClient.get(route, {observe:"response", responseType:"text"}).pipe(map(x=>{
      // this is a little baroque
      // for technical reasons internal to GeoOptix, the contents of the time series data.json
      // are not valid json, but a comma-separated list of json objects with a trailing comma.
      // (You can see this in your browser console if you throw a breakpoint here.)
      // So, we wrap the raw content in JSON syntax, making sure to account for that last comma,
      // before parsing and returning the object. The httpOption observe:"response" gives us the
      // entire HttpResponse, and responseType:"text" ensures that x.body is the raw content.
      return JSON.parse('{"data": [' + x.body.slice(0, -1) + ']}');
    }));
  }

  public downloadTimeSeriesCsv(wellCanonicalName: string, sensorCanonicalName: string, folderCanonicalName: string): Observable<any>{
    const route = `${environment.geooptixHostName}/projects/water-data-program/sites/${wellCanonicalName}/stations/${sensorCanonicalName}/folders/${folderCanonicalName}/files/data.csv/download`

    return this.httpClient.get(route, {responseType:"text"});
  }
}
