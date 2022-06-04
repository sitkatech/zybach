
import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterLevelInspectionService } from 'src/app/shared/generated/api/water-level-inspection.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterLevelInspectionSimpleDto } from 'src/app/shared/generated/model/water-level-inspection-simple-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-level-inspection-detail',
  templateUrl: './water-level-inspection-detail.component.html',
  styleUrls: ['./water-level-inspection-detail.component.scss']
})
export class WaterLevelInspectionDetailComponent implements OnInit {
  @ViewChild('deleteInspectionModal') deleteEntity: any;

  public currentUser: UserDto;
  public waterLevelInspectionID: number;
  public waterLevelInspection: WaterLevelInspectionSimpleDto;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;
  public inspectionIDToDelete: number;

  constructor(
    private waterLevelInspectionService: WaterLevelInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private modalService: NgbModal,
    private datePipe: DatePipe,
    private decimalPipe: DecimalPipe,
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.waterLevelInspectionID = parseInt(this.route.snapshot.paramMap.get("id"));
      forkJoin({
        waterLevelInspection: this.waterLevelInspectionService.waterLevelInspectionsWaterLevelInspectionIDGet(this.waterLevelInspectionID)
      }).subscribe(({ waterLevelInspection }) => {
        this.waterLevelInspection = waterLevelInspection;
        this.cdr.detectChanges();
      });
    })
  }

  public launchDeleteInspectionModal(waterLevelInspectionID: number): void {
    this.inspectionIDToDelete = waterLevelInspectionID;
    this.modalReference = this.modalService.open(this.deleteEntity, { ariaLabelledBy: 'deleteAnnouncementEntity', beforeDismiss: () => this.checkIfSubmitting(), backdrop: 'static', keyboard: false });
    this.modalReference.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed`;
    });
  }

  public checkIfSubmitting(): boolean {
    return !this.isPerformingAction;
  }

  public deleteInspectionByID(): void {
    this.isPerformingAction = true;
    this.waterLevelInspectionService.waterLevelInspectionsWaterLevelInspectionIDDelete(this.inspectionIDToDelete).subscribe(() => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.router.navigateByUrl("/water-level-inspections").then(() => {
        this.alertService.pushAlert(new Alert(`Inspection record successfully deleted`, AlertContext.Success, true));
      });
    }, error => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`There was an error deleting the inspection. Please try again`, AlertContext.Danger, true));
    })
  }

  ngOnDestroy() {
    this.cdr.detach();
  }

  public currentUserIsAdmin(): boolean {
    return this.authenticationService.isUserAnAdministrator(this.currentUser);
  }

}
