import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChemigationPermitUpsertDto } from '../shared/models/chemigation-permit-upsert-dto';
import { ChemigationPermitDto } from '../shared/models/generated/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from '../shared/models/generated/chemigation-permit-status-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class ChemigationInspectionService {

  constructor(private apiService: ApiService) { }

  public getInspectionSummaries(): Observable<any[]>{
    return this.apiService.getFromApi(`/chemigation/summaries`);
  }

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
  
  public createNewChemigationPermit(chemigationPermitToCreate: ChemigationPermitUpsertDto): Observable<ChemigationPermitDto> {
    let route = `/chemigationPermits`;
    return this.apiService.postToApi(route, chemigationPermitToCreate);
  }
  
  public updateChemigationPermitByID(chemigationPermitID: number, chemigationPermitUpdateDto: ChemigationPermitUpsertDto): Observable<ChemigationPermitDto> {
    let route = `/customPages/${chemigationPermitID}`;
    return this.apiService.putToApi(route, chemigationPermitUpdateDto);
  }

  public deleteChemigationPermitByID(chemigationPermitID: number): Observable<any> {
    let route = `/customPages/${chemigationPermitID}`;
    return this.apiService.deleteToApi(route);
  }
}
