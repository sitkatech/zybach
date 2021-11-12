import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorSummaryDto, WellWithSensorMessageAgeDto } from '../shared/models/well-with-sensor-summary-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class SensorStatusService {

  constructor(private apiService: ApiService) { }

  public getSensorStatusByWell(): Observable<WellWithSensorMessageAgeDto[]>{
    return this.apiService.getFromApi(`/sensorStatus`);
  }

  public getSensorStatusForWell(wellRegistrationID : string): Observable<WellWithSensorMessageAgeDto>{
    return this.apiService.getFromApi(`/sensorStatus/${wellRegistrationID}`);
  }

  public updateSensorIsActive(sensorSummaryDto: SensorSummaryDto){
    let route = `/sensorStatus/enableDisable`;
    return this.apiService.putToApi(route, sensorSummaryDto);
  }
}
