import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorAnomalySimpleDto } from '../shared/generated/model/sensor-anomaly-simple-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class SensorAnomalyService {

  constructor(
    private apiService: ApiService
  ) { }

  getSensorAnomalies(): Observable<Array<SensorAnomalySimpleDto>> {
    let route = `/sensorAnomalies`;
    return this.apiService.getFromApi(route);
  }
}
