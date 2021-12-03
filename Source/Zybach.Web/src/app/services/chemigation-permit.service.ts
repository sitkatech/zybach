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
import { ChemicalFormulationDto } from '../shared/generated/model/chemical-formulation-dto';
import { ChemicalUnitDto } from '../shared/generated/model/chemical-unit-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationSimpleDto } from '../shared/generated/model/chemigation-permit-annual-record-chemical-formulation-simple-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from '../shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { ChemicalFormulationYearlyTotalDto } from '../shared/generated/model/chemical-formulation-yearly-total-dto';
import { ChemigationPermitDetailedDto } from '../shared/generated/model/chemigation-permit-detailed-dto';

@Injectable({
  providedIn: 'root'
})
export class ChemigationPermitService {

  constructor(private apiService: ApiService) { }
 
  public getChemigationPermitByPermitNumber(chemigationPermitNumber: number): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits/${chemigationPermitNumber}`;
    return this.apiService.getFromApi(route);
  }
  
  public getChemigationPermits(): Observable<Array<ChemigationPermitDetailedDto>> {
    let route = `/chemigationPermits`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationPermitStatuses(): Observable<Array<ChemigationPermitStatusDto>> {
    let route = `/chemigationPermitStatuses`;
    return this.apiService.getFromApi(route);
  }

  public getCounties(): Observable<Array<ChemigationCountyDto>> {
    let route = `/counties`;
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

  public getLatestAnnualRecordByPermitNumber(chemigationPermitNumber: number): Observable<ChemigationPermitAnnualRecordDetailedDto> {
    let route = `/chemigationPermits/${chemigationPermitNumber}/getLatestRecordYear`;
    return this.apiService.getFromApi(route);
  }

  public getAnnualRecordByPermitNumberAndRecordYear(chemigationPermitNumber: number, recordYear: number): Observable<ChemigationPermitAnnualRecordDetailedDto> {
    let route = `/chemigationPermits/${chemigationPermitNumber}/${recordYear}`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationPermitAnnualRecordsByPermitNumber(chemigationPermitNumber: number): Observable<Array<ChemigationPermitAnnualRecordDetailedDto>> {
    let route = `/chemigationPermits/${chemigationPermitNumber}/annualRecords`;
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

  public getChemicalFormulations(): Observable<Array<ChemicalFormulationDto>> {
    let route = `/chemicalFormulations`;
    return this.apiService.getFromApi(route);
  }

  public getChemicalUnits(): Observable<Array<ChemicalUnitDto>> {
    let route = `/chemicalUnits`;
    return this.apiService.getFromApi(route);
  }
  
  public getChemicalFormulationsByPermitNumberAndRecordYear(chemigationPermitNumber: number, recordYear: number): Observable<Array<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>> {
    let route = `/chemigationPermits/${chemigationPermitNumber}/${recordYear}/chemicalFormulations`;
    return this.apiService.getFromApi(route);
  }

  public getChemicalFormulationYearlyTotals(): Observable<Array<ChemicalFormulationYearlyTotalDto>> {
    let route = `/chemicalFormulationYearlyTotals/`;
    return this.apiService.getFromApi(route);
  }
}
