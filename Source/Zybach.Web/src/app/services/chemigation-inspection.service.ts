import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class ChemigationInspectionService {

  constructor(private apiService: ApiService) { }

  public getInspectionSummaries(): Observable<any[]>{
    return this.apiService.getFromApi(`/chemigation/summaries`);
  }
}
