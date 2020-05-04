import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SiteDto } from '../shared/models/geooptix/site-dto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SiteService {

  constructor(private httpClient: HttpClient) { }

  public getSites(): Observable<SiteDto[]>{
    const route = `${environment.geooptixHostName}/sites`;

    return this.httpClient.get<SiteDto[]>(route);
  }

}
