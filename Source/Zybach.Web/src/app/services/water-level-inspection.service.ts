import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WaterLevelInspectionSimpleDto } from '../shared/generated/model/water-level-inspection-simple-dto';
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
}
 