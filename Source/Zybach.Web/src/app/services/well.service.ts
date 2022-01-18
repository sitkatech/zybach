import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChemigationPermitDetailedDto } from '../shared/generated/model/chemigation-permit-detailed-dto';
import { CountyDto } from '../shared/generated/model/county-dto';
import { InstallationRecordDto } from '../shared/generated/model/installation-record-dto';
import { WaterLevelInspectionSummaryDto } from '../shared/generated/model/water-level-inspection-summary-dto';
import { WaterQualityInspectionSummaryDto } from '../shared/generated/model/water-quality-inspection-summary-dto';
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

  public getChartData(wellID: number): Observable<WellChartDataDto> {
    return this.apiService.getFromApi(`chartData/${wellID}`);
  }

  public getWellDetails(wellID: number): Observable<WellDetailDto> {
    return this.apiService.getFromApi(`wells/${wellID}`);
  }

  public getWellUses(): Observable<Array<WellUseDto>> {
    return this.apiService.getFromApi(`wellUses`);
  }

  public getWellParticipations(): Observable<Array<WellParticipationDto>> {
    return this.apiService.getFromApi(`wellParticipations`);
  }

  public updateWellRegistrationID(wellID: number, newWellRegistrationIDDto: WellRegistrationIDDto): Observable<WellSimpleDto> {
    return this.apiService.putToApi(`wells/${wellID}/editRegistrationID`, newWellRegistrationIDDto);
  }

  public getWellContactDetails(wellID: number): Observable<WellContactInfoDto> {
    return this.apiService.getFromApi(`wells/${wellID}/contactInfo`);
  }

  public updateWellContactDetails(wellID: number, wellContactInfoDto: WellContactInfoDto): Observable<any> {
    return this.apiService.putToApi(`wells/${wellID}/contactInfo`, wellContactInfoDto);
  }

  public getWellParticipationDetails(wellID: number): Observable<WellParticipationInfoDto> {
    return this.apiService.getFromApi(`wells/${wellID}/participationInfo`);
  }

  public updateWellParticipationDetails(wellID: number, wellParticipationInfoDto: WellParticipationInfoDto): Observable<any> {
    return this.apiService.putToApi(`wells/${wellID}/participationInfo`, wellParticipationInfoDto);
  }

  public getCounties(): Observable<Array<CountyDto>> {
    return this.apiService.getFromApi(`/counties`);
  }

  public getInstallationDetails(wellID: number): Observable<InstallationRecordDto[]> {
    return this.apiService.getFromApi(`wells/${wellID}/installation`);
  }

  public getChemigationPermits(wellID: number): Observable<Array<ChemigationPermitDetailedDto>> {
    return this.apiService.getFromApi(`wells/${wellID}/chemigationPermits`);
  }

  public getPhoto(
    wellID: number,
    installationCanonicalName: string,
    photoCanonicalName: any
  ): Observable<any> {
    const route = `${environment.mainAppApiUrl}/wells/${wellID}/installation/${installationCanonicalName}/photo/${photoCanonicalName}`
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

  public listWaterLevelInspectionsByWellID(wellID: number): Observable<Array<WaterLevelInspectionSummaryDto>> {
    let route = `wells/${wellID}/getWaterLevelInspectionSummaries`;
    return this.apiService.getFromApi(route);
  }

  public listWaterQualityInspectionsByWellID(wellID: number): Observable<Array<WaterQualityInspectionSummaryDto>> {
    let route = `wells/${wellID}/getWaterQualityInspectionSummaries`;
    return this.apiService.getFromApi(route);
  }

}
