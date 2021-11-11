import { Injectable } from '@angular/core';
import { ApiService } from '../shared/services';
import { Observable } from 'rxjs';
import { ChemigationPermitUpsertDto } from '../shared/models/chemigation-permit-upsert-dto';
import { ChemigationPermitDto } from '../shared/models/generated/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from '../shared/models/generated/chemigation-permit-status-dto';
import { ChemigationPermitAnnualRecordDto } from '../shared/models/generated/chemigation-permit-annual-record-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from '../shared/models/chemigation-permit-annual-record-upsert-dto';
import { ChemigationPermitNewDto } from '../shared/models/chemigation-permit-new-dto';


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

  public getChemigationPermitAnnualRecordsByPermitID(chemigationPermitID: number): Observable<Array<ChemigationPermitAnnualRecordDto>> {
    let route = `/chemigationPermitAnnualRecords/getByPermitID/${chemigationPermitID}`;
    return this.apiService.getFromApi(route);
  }
  
  public createChemigationPermitAnnualRecord(chemigationPermitAnnualRecord: ChemigationPermitAnnualRecordUpsertDto): Observable<ChemigationPermitAnnualRecordDto> {
    let route = `/chemigationPermitAnnualRecords`;
    return this.apiService.postToApi(route, chemigationPermitAnnualRecord);
  }
}
