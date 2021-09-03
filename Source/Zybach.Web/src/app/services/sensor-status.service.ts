import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WellWithSensorMessageAgeDto } from '../shared/models/well-with-sensor-summary-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class SensorStatusService {

  constructor(private apiService: ApiService) { }

  public getSensorStatusByWell(): Observable<WellWithSensorMessageAgeDto[]>{
    return this.apiService.getFromApi(`/sensorStatus`);
  }
}
