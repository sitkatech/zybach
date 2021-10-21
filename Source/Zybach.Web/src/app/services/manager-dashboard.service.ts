import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DistrictStatisticsDto } from '../shared/models/district-statistics-dto';
import { StreamFlowZoneDto } from '../shared/models/stream-flow-zone-dto';
import { StreamFlowZonePumpingDepthDto } from '../shared/models/stream-flow-zone-pumping-depth-dto';
import { ApiService } from '../shared/services';

@Injectable({
  providedIn: 'root'
})
export class ManagerDashboardService {

  constructor(private apiService: ApiService) { }

  getDistrictStatistics(): Observable<DistrictStatisticsDto>{
    return this.apiService.getFromApi(`managerDashboard/districtStatistics`);
  }

  getStreamflowZones(): Observable<StreamFlowZoneDto[]> {
    return this.apiService.getFromApi(`streamFlowZones`);
  }

  getStreamFlowZonePumpingDepths(): Observable<{Year:number, StreamFlowZonePumpingDepths: StreamFlowZonePumpingDepthDto[]}[]> {
    return this.apiService.getFromApi(`managerDashboard/streamFlowZonePumpingDepths`)
  }
}
