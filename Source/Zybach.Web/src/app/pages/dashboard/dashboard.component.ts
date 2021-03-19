import { Component, OnInit } from '@angular/core';
import { ManagerDashboardService } from 'src/app/services/manager-dashboard.service';
import { DistrictStatisticsDto } from 'src/app/shared/models/district-statistics-dto';

@Component({
  selector: 'zybach-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public yearToDisplay: number;
  public districtStatistics: DistrictStatisticsDto;
  public loadingDistrictStatistics: boolean = true;

  constructor(
    private managerDashboardService: ManagerDashboardService
  ) { }

  ngOnInit(): void {
    this.yearToDisplay = new Date().getFullYear();
  }

  public updateAnnualData(): void {
    this.loadingDistrictStatistics = true;
    this.managerDashboardService.getDistrictStatistics(this.yearToDisplay).subscribe(stats =>{
      this.districtStatistics = stats;
      this.loadingDistrictStatistics = false;
    })

  }

}
