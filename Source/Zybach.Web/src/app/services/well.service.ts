import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChemigationPermitAnnualRecordDetailedDto } from '../shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { InstallationRecordDto } from '../shared/generated/model/installation-record-dto';
import { WellChartDataDto } from '../shared/generated/model/well-chart-data-dto';
import { WellDetailDto } from '../shared/generated/model/well-detail-dto';
import { WellNewDto } from '../shared/generated/model/well-new-dto';
import { WellSimpleDto } from '../shared/generated/model/well-simple-dto';
import { WellWithSensorSummaryDto } from '../shared/generated/model/well-with-sensor-summary-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class WellService {
  constructor(
    private apiService: ApiService,
    private httpClient: HttpClient
  ) { }

  public getWellsMapData(): Observable<WellWithSensorSummaryDto[]> {
    return this.apiService.getFromApi("mapData/wells")
  }

  public getChartData(wellRegistrationID: string): Observable<WellChartDataDto> {
    return this.apiService.getFromApi(`chartData/${wellRegistrationID}`);
  }

  public getWellDetails(wellRegistrationID: string): Observable<WellDetailDto> {
    return this.apiService.getFromApi(`wells/${wellRegistrationID}/details`);
  }

  public getInstallationDetails(wellRegistrationID: string): Observable<InstallationRecordDto[]> {
    return this.apiService.getFromApi(`wells/${wellRegistrationID}/installation`);
  }

  public getChemigationPermts(wellRegistrationID: string): Observable<Array<ChemigationPermitAnnualRecordDetailedDto>> {
    return this.apiService.getFromApi(`wells/${wellRegistrationID}/chemigationPermits`);
  }

  public getPhoto(
    wellRegistrationID: string,
    installationCanonicalName: string,
    photoCanonicalName: any
  ): Observable<any> {
    const route = `${environment.mainAppApiUrl}/wells/${wellRegistrationID}/installation/${installationCanonicalName}/photo/${photoCanonicalName}`
    return this.httpClient.get(route, { responseType: "blob" });
  }

  public getRobustReviewScenarioJson(): Observable<any> {
    const route = `${environment.mainAppApiUrl}/wells/download/robustReviewScenarioJson`
    return this.httpClient.get(route, { responseType: "blob" as "json" });
  }

  public newWell(wellNewDto: WellNewDto) {
    let route = `/wells/new`;
    return this.apiService.postToApi(route, wellNewDto);
  }

  public searchByWellRegistrationID(wellRegistrationID: string): Observable<WellSimpleDto> {
    let route = `/wells/search/${wellRegistrationID}`;
    return this.apiService.getFromApi(route);
  }
}
