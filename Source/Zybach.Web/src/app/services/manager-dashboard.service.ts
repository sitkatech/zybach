import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DistrictStatisticsDto } from '../shared/models/district-statistics-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class ManagerDashboardService {

  constructor(private apiService: ApiService) { }

  getDistrictStatistics(year: number): Observable<DistrictStatisticsDto>{
    return this.apiService.getFromApi(`managerDashboard/districtStatistics/${year}`);
  }
}
