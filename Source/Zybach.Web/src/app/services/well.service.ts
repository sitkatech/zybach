import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SiteDto } from '../shared/models/geooptix/site-dto';
import { environment } from 'src/environments/environment';
import { map, filter, switchMap } from 'rxjs/operators';


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

    return this.httpClient.get<SiteDto>(route, {observe:"response"}).pipe(map(response => {
      
      if(response.status != 200) { 
      throw new Error( response.status + ' No Content'); } // If everything went fine, return the response 
      else {
       return response.body; 
      } 
    })
    );
  }

}
