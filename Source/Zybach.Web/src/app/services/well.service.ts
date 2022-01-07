import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChemigationPermitAnnualRecordDetailedDto } from '../shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { CountyDto } from '../shared/generated/model/county-dto';
import { InstallationRecordDto } from '../shared/generated/model/installation-record-dto';
import { WellChartDataDto } from '../shared/generated/model/well-chart-data-dto';
import { WellContactInfoDto } from '../shared/generated/model/well-contact-info-dto';
import { WellDetailDto } from '../shared/generated/model/well-detail-dto';
import { WellNewDto } from '../shared/generated/model/well-new-dto';
import { WellParticipationDto } from '../shared/generated/model/well-participation-dto';
import { WellParticipationInfoDto } from '../shared/generated/model/well-participation-info-dto';
import { WellRegistrationIDDto } from '../shared/generated/model/well-registration-id-dto';
import { WellSimpleDto } from '../shared/generated/model/well-simple-dto';
import { WellUseDto } from '../shared/generated/model/well-use-dto';
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
    return this.apiService.getFromApi(`wells/${wellRegistrationID}`);
  }

  public getWellUses(): Observable<Array<WellUseDto>> {
    return this.apiService.getFromApi(`wells/wellUses`);
  }

  public getWellParticipations(): Observable<Array<WellParticipationDto>> {
    return this.apiService.getFromApi(`wells/wellParticipations`);
  }

  public updateWellRegistrationID(wellRegistrationID: string, newWellRegistrationIDDto: WellRegistrationIDDto): Observable<WellSimpleDto> {
    return this.apiService.putToApi(`wells/${wellRegistrationID}/editRegistrationID`, newWellRegistrationIDDto);
  }

  public getWellContactDetails(wellRegistrationID: string): Observable<WellContactInfoDto> {
    return this.apiService.getFromApi(`wells/${wellRegistrationID}/contactInfo`);
  }

  public updateWellContactDetails(wellRegistrationID: string, wellContactInfoDto: WellContactInfoDto): Observable<any> {
    return this.apiService.putToApi(`wells/${wellRegistrationID}/contactInfo`, wellContactInfoDto);
  }

  public getWellParticipationDetails(wellRegistrationID: string): Observable<WellParticipationInfoDto> {
    return this.apiService.getFromApi(`wells/${wellRegistrationID}/participationInfo`);
  }

  public updateWellParticipationDetails(wellRegistrationID: string, wellParticipationInfoDto: WellParticipationInfoDto): Observable<any> {
    return this.apiService.putToApi(`wells/${wellRegistrationID}/participationInfo`, wellParticipationInfoDto);
  }

  public getCounties(): Observable<Array<CountyDto>> {
    return this.apiService.getFromApi(`/counties`);
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

  public newWell(wellNewDto: WellNewDto) {
    let route = `/wells/new`;
    return this.apiService.postToApi(route, wellNewDto);
  }

  public searchByWellRegistrationID(wellRegistrationID: string): Observable<Array<string>> {
    let route = `/wells/search/${wellRegistrationID}`;
    return this.apiService.getFromApi(route);
  }

  public searchByWellRegistrationIDHasInspectionType(wellRegistrationID: string): Observable<Array<string>> {
    let route = `/wells/search/${wellRegistrationID}/hasInspectionType`;
    return this.apiService.getFromApi(route);
  }
}
