import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class WellService {

  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient
    ) { }

  public getWells(): Observable<any> {
    return this.apiService.getFromApi("wells");
  }

  public getWellsMapData(): Observable<any> {
    return this.apiService.getFromApi("mapData/wells")
  }

  public getWell(id: string): Observable<any> {
    return this.apiService.getFromApi(`wells/${id}`);
  }

  public getChartData(id: string): Observable<any> {
    return this.apiService.getFromApi(`chartData/${id}`);
  }

  public getWellDetails(id: string): Observable<any> {
    return this.apiService.getFromApi(`wells/${id}/details`);
  }

  public getInstallationDetails(id: string): Observable<any> {
    return this.apiService.getFromApi(`wells/${id}/installation`);
  }

  public getPhoto(
    wellRegistrationID: string,
    installationCanonicalName: string,
    photoCanonicalName: any
  ): Observable<any> {
    const route = `https://${environment.apiHostName}/wells/${wellRegistrationID}/installation/${installationCanonicalName}/photo/${photoCanonicalName}`
    return this.httpClient.get(route, {responseType: "blob"});
  }
}
