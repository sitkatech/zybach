import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { WaterQualityInspectionSimpleDto } from 'src/app/shared/generated/model/water-quality-inspection-simple-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-water-quality-inspection-detail',
  templateUrl: './water-quality-inspection-detail.component.html',
  styleUrls: ['./water-quality-inspection-detail.component.scss']
})
export class WaterQualityInspectionDetailComponent implements OnInit, OnDestroy {
  @ViewChild('deleteInspectionModal') deleteEntity: any;
  
  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public waterQualityInspectionID: number;
  public waterQualityInspection: WaterQualityInspectionSimpleDto;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;
  public inspectionIDToDelete: number;

  constructor(
    private waterQualityInspectionService: WaterQualityInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.waterQualityInspectionID = parseInt(this.route.snapshot.paramMap.get("id"));
      forkJoin({
        waterQualityInspection: this.waterQualityInspectionService.getByID(this.waterQualityInspectionID)
      }).subscribe(({ waterQualityInspection}) => {
        this.waterQualityInspection = waterQualityInspection;
        this.cdr.detectChanges();
      });
    })
  }

  public launchDeleteInspectionModal(waterQualityInspectionID: number): void {
    this.inspectionIDToDelete = waterQualityInspectionID;
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
    this.waterQualityInspectionService.deleteWaterQualityInspection(this.inspectionIDToDelete).subscribe(() => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.router.navigateByUrl("/water-quality-inspections").then(() => {
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