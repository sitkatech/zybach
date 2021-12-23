import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WaterQualityInspectionSimpleDto } from '../shared/generated/model/water-quality-inspection-simple-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class WaterQualityInspectionService {

  constructor(private apiService: ApiService) { }
 
  public getWaterQualityInspections(): Observable<Array<WaterQualityInspectionSimpleDto>> {
    let route = `/waterQualityInspections`;
    return this.apiService.getFromApi(route);
  }
}
