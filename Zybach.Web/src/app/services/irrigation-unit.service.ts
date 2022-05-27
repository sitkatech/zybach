import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { AgHubIrrigationUnitDetailDto } from '../shared/generated/model/ag-hub-irrigation-unit-detail-dto';
import { AgHubIrrigationUnitSimpleDto } from '../shared/generated/model/ag-hub-irrigation-unit-simple-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class IrrigationUnitService {

  constructor(
    private apiService: ApiService
  ) { }

  public getIrrigationUnitDetailsByID(irrigationUnitID: number): Observable<AgHubIrrigationUnitDetailDto> {
    return this.apiService.getFromApi(`irrigationUnits/${irrigationUnitID}`);
  }

  public listIrrigationUnits(): Observable<AgHubIrrigationUnitSimpleDto[]> {
    return this.apiService.getFromApi(`irrigationUnits/`);
  }

}
