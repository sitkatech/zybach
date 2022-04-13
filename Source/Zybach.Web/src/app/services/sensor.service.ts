import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorSimpleDto } from '../shared/generated/model/sensor-simple-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class SensorService {

  constructor(private apiService: ApiService) { }

  public listSensors(): Observable<Array<SensorSimpleDto>> {
    return this.apiService.getFromApi(`/sensors`);
  }

  public getSensorByID(sensorID: number): Observable<SensorSimpleDto> {
    return this.apiService.getFromApi(`/sensors/${sensorID}`);
  }

  public listSensorsByWellID(wellID: number): Observable<Array<SensorSimpleDto>> {
    return this.apiService.getFromApi(`/sensors/byWellID/${wellID}`);
  }
}


