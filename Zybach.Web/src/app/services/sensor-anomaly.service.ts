import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorAnomalySimpleDto } from '../shared/generated/model/sensor-anomaly-simple-dto';
import { SensorAnomalyUpsertDto } from '../shared/generated/model/sensor-anomaly-upsert-dto';
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

  getSensorAnomalyByID(sensorAnomalyID: number): Observable<SensorAnomalySimpleDto> {
    let route = `/sensorAnomalies/${sensorAnomalyID}`;
    return this.apiService.getFromApi(route);
  }

  createSensorAnomaly(sensorAnomalyUpsertDto: SensorAnomalyUpsertDto): Observable<any> {
    let route = `/sensorAnomalies/new`;
    return this.apiService.postToApi(route, sensorAnomalyUpsertDto);
  }

  updateSensorAnomaly(sensorAnomalyUpsertDto: SensorAnomalyUpsertDto): Observable<any> {
    let route = `/sensorAnomalies/update`;
    return this.apiService.postToApi(route, sensorAnomalyUpsertDto); 
  }

  deleteSensorAnomaly(sensorAnomalyID: number): Observable<any> {
    let route = `/sensorAnomalies/${sensorAnomalyID}`;
    return this.apiService.deleteToApi(route);
  }
}
