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
import { ChemigationInterlockTypeDto } from '../shared/generated/model/chemigation-interlock-type-dto';

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
    let route = `/chemigationInspections/inspectionTypes`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInspectionStatuses(): Observable<Array<ChemigationInspectionStatusDto>> {
    let route = `/chemigationInspections/inspectionStatuses`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInspectionFailureReasons(): Observable<Array<ChemigationInspectionFailureReasonDto>> {
    let route = `/chemigationInspections/failureReasons`;
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
    let route = `/chemigationInspections/mainlineCheckValves`;
    return this.apiService.getFromApi(route);
  }

  public getLowPressureValves(): Observable<Array<ChemigationLowPressureValveDto>> {
    let route = `/chemigationInspections/lowPressureValves`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInjectionValves(): Observable<Array<ChemigationInjectionValveDto>> {
    let route = `/chemigationInspections/injectionValves`;
    return this.apiService.getFromApi(route);
  }

  public getChemigationInterlockTypes(): Observable<Array<ChemigationInterlockTypeDto>> {
    let route = `/chemigationInspections/interlockTypes`;
    return this.apiService.getFromApi(route);
  }

  public createChemigationInspectionByAnnualRecordID(chemigationPermitAnnualRecordID: number, chemigationInspectionUpsertDto: ChemigationInspectionUpsertDto): Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationPermits/annualRecords/${chemigationPermitAnnualRecordID}/createInspection`;
    return this.apiService.postToApi(route, chemigationInspectionUpsertDto);
  }
  
  public updateChemigationInspectionByID(chemigationInspectionID: number, chemigationInspectionUpsertDto: ChemigationInspectionUpsertDto): Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.putToApi(route, chemigationInspectionUpsertDto);

  }
  public deleteChemigationInspectionByID(chemigationInspectionID: number) : any {
    let route = `/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.deleteToApi(route);
  }

  public getChemigationInspectionByID(chemigationInspectionID: number) : Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationInspections/${chemigationInspectionID}`;
    return this.apiService.getFromApi(route);
  }

  public getLatestChemigationInspectionByPermitNumber(chemigationPermitNumber: number) : Observable<ChemigationInspectionSimpleDto> {
    let route = `/chemigationPermits/${chemigationPermitNumber}/latestChemigationInspection`;
    return this.apiService.getFromApi(route);
  }
}
