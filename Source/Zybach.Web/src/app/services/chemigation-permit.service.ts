import { Injectable } from '@angular/core';
import { ApiService } from '../shared/services';
import { Observable } from 'rxjs';
import { ChemigationCountyDto } from '../shared/generated/model/chemigation-county-dto';
import { ChemigationInjectionUnitTypeDto } from '../shared/generated/model/chemigation-injection-unit-type-dto';
import { ChemigationPermitAnnualRecordDto } from '../shared/generated/model/chemigation-permit-annual-record-dto';
import { ChemigationPermitAnnualRecordStatusDto } from '../shared/generated/model/chemigation-permit-annual-record-status-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from '../shared/generated/model/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitDto } from '../shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitNewDto } from '../shared/generated/model/chemigation-permit-new-dto';
import { ChemigationPermitStatusDto } from '../shared/generated/model/chemigation-permit-status-dto';
import { ChemigationPermitUpsertDto } from '../shared/generated/model/chemigation-permit-upsert-dto';


@Injectable({
  providedIn: 'root'
})
export class ChemigationPermitService {

  constructor(private apiService: ApiService) { }

  public getChemigationPermitByID(chemigationPermitID: number): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits/getByID/${chemigationPermitID}`;
    return this.apiService.getFromApi(route);
  }
  
  public getChemigationPermitByPermitNumber(chemigationPermitNumber: number): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits/getByPermitNumber/${chemigationPermitNumber}`;
    return this.apiService.getFromApi(route);
  }
  
  public getAllChemigationPermits(): Observable<Array<ChemigationPermitDto>> {
    let route = `/chemigationPermits`;
    return this.apiService.getFromApi(route);
  }

  public getAllChemigationPermitStatuses(): Observable<Array<ChemigationPermitStatusDto>> {
    let route = `/chemigationPermits/permitStatuses`;
    return this.apiService.getFromApi(route);
  }

  public getAllChemigationCounties(): Observable<Array<ChemigationCountyDto>> {
    let route = `/chemigationPermits/chemigationCounties`;
    return this.apiService.getFromApi(route);
  }
  
  public createNewChemigationPermit(chemigationPermitToCreate: ChemigationPermitNewDto): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits`;
    return this.apiService.postToApi(route, chemigationPermitToCreate);
  }
  
  public updateChemigationPermitByID(chemigationPermitID: number, chemigationPermitUpsertDto: ChemigationPermitUpsertDto): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits/${chemigationPermitID}`;
    return this.apiService.putToApi(route, chemigationPermitUpsertDto);
  }

  public deleteChemigationPermitByID(chemigationPermitID: number): Observable<any> {
    let route = `/chemigationPermits/${chemigationPermitID}`;
    return this.apiService.deleteToApi(route);
  }

  public getLatestAnnualRecordByPermitNumber(chemigationPermitNumber: number): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/getByPermitNumber/${chemigationPermitNumber}/annualRecords/getLatestRecordYear`;
    return this.apiService.getFromApi(route);
  }

  public getLatestAnnualRecordByPermitID(chemigationPermitID: number): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/getByID/${chemigationPermitID}/annualRecords/getLatestRecordYear`;
    return this.apiService.getFromApi(route);
  }

  public getAnnualRecordByPermitNumberAndRecordYear(chemigationPermitNumber: number, recordYear: number): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/getByPermitNumber/${chemigationPermitNumber}/annualRecords/${recordYear}`;
    return this.apiService.getFromApi(route);
  }

  public getAnnualRecordByPermitIDAndRecordYear(chemigationPermitID: number, recordYear: number): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/getByPermitNumber/${chemigationPermitID}/annualRecords/${recordYear}`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationPermitAnnualRecordsByPermitID(chemigationPermitID: number): Observable<Array<ChemigationPermitAnnualRecordDto>> {
    let route = `/chemigationPermits/getByID/${chemigationPermitID}/annualRecords`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationPermitAnnualRecordsByPermitNumber(chemigationPermitNumber: number): Observable<Array<ChemigationPermitAnnualRecordDto>> {
    let route = `/chemigationPermits/getByPermitNumber/${chemigationPermitNumber}/annualRecords`;
    return this.apiService.getFromApi(route);
  }

  public getAllChemigationInjectionUnitTypes(): Observable<Array<ChemigationInjectionUnitTypeDto>> {
    let route = `/chemigationPermits/injectionUnitTypes`;
    return this.apiService.getFromApi(route);
  }

  public getAnnualRecordStatusTypes(): Observable<Array<ChemigationPermitAnnualRecordStatusDto>> {
    let route = `/chemigationPermits/annualRecords/annualRecordStatuses`;
    return this.apiService.getFromApi(route);
  }
  
  public createChemigationPermitAnnualRecord(chemigationPermitID:number, chemigationPermitAnnualRecord: ChemigationPermitAnnualRecordUpsertDto): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/${chemigationPermitID}/annualRecords`;
    return this.apiService.postToApi(route, chemigationPermitAnnualRecord);
  }

  public updateChemigationPermitAnnualRecord(annualRecordID: number, annualRecordUpsertDto: ChemigationPermitAnnualRecordUpsertDto): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermits/annualRecords/${annualRecordID}`;
    return this.apiService.putToApi(route, annualRecordUpsertDto);
  }
}
