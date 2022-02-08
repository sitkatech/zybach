import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WaterLevelInspectionSimpleDto } from '../shared/generated/model/water-level-inspection-simple-dto';
import { WaterLevelInspectionUpsertDto } from '../shared/generated/model/water-level-inspection-upsert-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class WaterLevelInspectionService {

  constructor(private apiService: ApiService) { }

  public getWaterLevelInspections(): Observable<Array<WaterLevelInspectionSimpleDto>> {
    let route = `/waterLevelInspections`;
    return this.apiService.getFromApi(route);
  }

  public getByID(waterLevelInspectionID: number): Observable<WaterLevelInspectionSimpleDto> {
    let route = `/waterLevelInspections/${waterLevelInspectionID}`;
    return this.apiService.getFromApi(route);
  }  

  public createWaterLevelInspection(waterLevelInspectionUpsertDto: WaterLevelInspectionUpsertDto): Observable<WaterLevelInspectionSimpleDto> {
    let route = `/waterLevelInspections`;
    return this.apiService.postToApi(route, waterLevelInspectionUpsertDto);
  }

  public updateWaterLevelInspection(waterLevelInspectionID: number, waterLevelInspectionUpsertDto: WaterLevelInspectionUpsertDto): Observable<any> {
    let route = `/waterLevelInspections/${waterLevelInspectionID}`;
    return this.apiService.putToApi(route, waterLevelInspectionUpsertDto);
  }

  public deleteWaterLevelInspection(waterLevelInspectionID: number): Observable<any> {
    let route = `/waterLevelInspections/${waterLevelInspectionID}`;
    return this.apiService.deleteToApi(route);
  }
}
 