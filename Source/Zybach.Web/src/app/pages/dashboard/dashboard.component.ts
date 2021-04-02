import { Component, OnInit } from '@angular/core';
import { ManagerDashboardService } from 'src/app/services/manager-dashboard.service';
import { DistrictStatisticsDto } from 'src/app/shared/models/district-statistics-dto';

@Component({
  selector: 'zybach-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;
  
  public districtStatistics: DistrictStatisticsDto;
  public loadingDistrictStatistics: boolean = true;

  constructor(
    private managerDashboardService: ManagerDashboardService
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear();
  }

  public updateAnnualData(): void {
    this.loadingDistrictStatistics = true;

    debugger;
    const yearForStatistics = this.allYearsSelected ? this.currentYear : this.yearToDisplay;

    this.managerDashboardService.getDistrictStatistics(yearForStatistics).subscribe(stats =>{
      this.districtStatistics = stats;
      this.loadingDistrictStatistics = false;
    });
  }
}
