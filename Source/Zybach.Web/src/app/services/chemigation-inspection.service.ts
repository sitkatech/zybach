import { Injectable } from '@angular/core';
import { ApiService } from '../shared/services';
import { Observable } from 'rxjs';
import { ChemigationInspectionTypeDto } from '../shared/generated/model/chemigation-inspection-type-dto';
import { ChemigationInspectionStatusDto } from '../shared/generated/model/chemigation-inspection-status-dto';
import { ChemigationInspectionFailureReasonDto } from '../shared/generated/model/chemigation-inspection-failure-reason-dto';
import { TillageDto } from '../shared/generated/model/tillage-dto';
import { CropTypeDto } from '../shared/generated/model/crop-type-dto';
import { ChemigationMainlineCheckValveDto } from '../shared/generated/model/chemigation-mainline-check-valve-dto';
import { ChemigationLowPressureValveDto } from '../shared/generated/model/chemigation-low-pressure-valve-dto';
import { ChemigationInjectionValveDto } from '../shared/generated/model/chemigation-injection-valve-dto';
import { ChemigationInspectionUpsertDto } from '../shared/generated/model/chemigation-inspection-upsert-dto';
import { ChemigationInspectionSimpleDto } from '../shared/generated/model/chemigation-inspection-simple-dto';

@Injectable({
  providedIn: 'root'
})
export class ChemigationInspectionService {

  constructor(private apiService: ApiService) { }

  public getAllChemigationInspections(): Observable<Array<ChemigationInspectionSimpleDto>> {
    let route = `/chemigationInspections`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInspectionTypes(): Observable<Array<ChemigationInspectionTypeDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/inspectionTypes`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInspectionStatuses(): Observable<Array<ChemigationInspectionStatusDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/inspectionStatuses`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInspectionFailureReasons(): Observable<Array<ChemigationInspectionFailureReasonDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/failureReasons`;
    return this.apiService.getFromApi(route);
  }

  public getTillageTypes(): Observable<Array<TillageDto>> {
    let route = `/tillageTypes`;
    return this.apiService.getFromApi(route);
  }

  public getCropTypes(): Observable<Array<CropTypeDto>> {
    let route = `/cropTypes`;
    return this.apiService.getFromApi(route);
  }

  public getMainlineCheckValves(): Observable<Array<ChemigationMainlineCheckValveDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/mainlineCheckValves`;
    return this.apiService.getFromApi(route);
  }

  public getLowPressureValves(): Observable<Array<ChemigationLowPressureValveDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/lowPressureValves`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInjectionValves(): Observable<Array<ChemigationInjectionValveDto>> {
    let route = `/chemigationPermits/annualRecords/chemigationInspections/injectionValves`;
    return this.apiService.getFromApi(route);
  }

  public createChemigationInspectionByAnnualRecordID(chemigationPermitAnnualRecordID: number, chemigationInspectionUpsertDto: ChemigationInspectionUpsertDto): Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationPermits/annualRecords/${chemigationPermitAnnualRecordID}/createInspection`;
    return this.apiService.postToApi(route, chemigationInspectionUpsertDto);
  }
  
  public updateChemigationInspectionByAnnualRecordIDAndInspectionID(chemigationPermitAnnualRecordID: number, chemigationInspectionID: number, chemigationInspectionUpsertDto: ChemigationInspectionUpsertDto): Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationPermits/annualRecords/${chemigationPermitAnnualRecordID}/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.putToApi(route, chemigationInspectionUpsertDto);

  }
  public deleteChemigationInspectionByID(chemigationPermitAnnualRecordID: number, chemigationInspectionID: number) : any {
    let route = `chemigationPermits/annualRecords/${chemigationPermitAnnualRecordID}/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.deleteToApi(route);
  }

  public getChemigationInspectionByID(chemigationInspectionID: number) : Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.getFromApi(route);
  }
}
