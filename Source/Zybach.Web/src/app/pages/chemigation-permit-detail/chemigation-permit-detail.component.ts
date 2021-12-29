import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationInspectionSimpleDto } from 'src/app/shared/generated/model/chemigation-inspection-simple-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-detail',
  templateUrl: './chemigation-permit-detail.component.html',
  styleUrls: ['./chemigation-permit-detail.component.scss']
})

export class ChemigationPermitDetailComponent implements OnInit, OnDestroy {
  @ViewChild('deleteInspectionModal') deleteEntity: any;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public annualRecords: Array<ChemigationPermitAnnualRecordDetailedDto>;

  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;
  public currentYearAnnualRecord: ChemigationPermitAnnualRecordDetailedDto;

  public modalReference: NgbModalRef;
  public isPerformingAction: boolean = false;
  public closeResult: string;
  public inspectionIDToDelete: number;
  

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private chemigationInspectionService: ChemigationInspectionService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.yearToDisplay = new Date().getFullYear();    

    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => {
      this.currentUser = currentUser;
      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      forkJoin({
        chemigationPermit: this.chemigationPermitService.getChemigationPermitByPermitNumber(this.chemigationPermitNumber),
        annualRecords: this.chemigationPermitService.getChemigationPermitAnnualRecordsByPermitNumber(this.chemigationPermitNumber)
      }).subscribe(({ chemigationPermit, annualRecords}) => {
        this.chemigationPermit = chemigationPermit;
        this.annualRecords = annualRecords;

        this.updateAnnualData();
        this.cdr.detectChanges();
      });
    })
  }

  ngOnDestroy() {
    this.watchUserChangeSubscription.unsubscribe();
    this.authenticationService.dispose();
    this.cdr.detach();
  }

  public currentUserIsAdmin(): boolean {
    return this.authenticationService.isUserAnAdministrator(this.currentUser);
  }

  public permitHasCurrentRecord(): boolean {
    return this.annualRecords?.map(x => x.RecordYear).includes(this.currentYear);
  }
  
  public updateAnnualData(): void {
    this.currentYearAnnualRecord = this.annualRecords?.find(x => x.RecordYear == this.yearToDisplay);
  }

  public getInspections(): Array<ChemigationInspectionSimpleDto> {
    return this.currentYearAnnualRecord?.Inspections
    .sort((a, b) => a.ChemigationInspectionStatusID - b.ChemigationInspectionStatusID)
    .sort((a, b) => Date.parse(b.InspectionDate) - Date.parse(a.InspectionDate));
  }

  public launchDeleteInspectionModal(chemigationInspectionID: number): void {
    this.inspectionIDToDelete = chemigationInspectionID;
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
    this.chemigationInspectionService.deleteChemigationInspectionByID(this.inspectionIDToDelete).subscribe(() => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.router.onSameUrlNavigation = 'reload';
      this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermit.ChemigationPermitNumber).then(() => {
        this.updateAnnualData();
        this.alertService.pushAlert(new Alert(`Inspection record successfully deleted`, AlertContext.Success, true));
      });
    }, error => {
      this.modalReference.close();
      this.isPerformingAction = false;
      this.alertService.pushAlert(new Alert(`There was an error deleting the inspection. Please try again`, AlertContext.Danger, true));
    })
  }
}