import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class WellService {

  constructor(private apiService: ApiService) { }

  public getWells() : Observable<any>{
    return this.apiService.getFromApi("wells");
  }

  public getWellsMapData(): Observable<any> {
    return this.apiService.getFromApi("mapData/wells")
  }
  
  public getWell(id: string): Observable<any> {
    return this.apiService.getFromApi(`wells/${id}`);
  }
}
