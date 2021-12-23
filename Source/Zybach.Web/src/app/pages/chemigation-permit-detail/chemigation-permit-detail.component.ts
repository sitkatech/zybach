import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationInspectionUpsertComponent } from 'src/app/shared/components/chemigation-inspection-upsert/chemigation-inspection-upsert.component';
import { ChemigationInspectionSimpleDto } from 'src/app/shared/generated/model/chemigation-inspection-simple-dto';
import { ChemigationInspectionUpsertDto } from 'src/app/shared/generated/model/chemigation-inspection-upsert-dto';
import { ChemigationPermitAnnualRecordDetailedDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-detailed-dto';
import { ChemigationPermitAnnualRecordDto } from 'src/app/shared/generated/model/chemigation-permit-annual-record-dto';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { ChemigationInspectionStatusEnum } from 'src/app/shared/models/enums/chemigation-inspection-status';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-chemigation-permit-detail',
  templateUrl: './chemigation-permit-detail.component.html',
  styleUrls: ['./chemigation-permit-detail.component.scss']
})
export class ChemigationPermitDetailComponent implements OnInit, OnDestroy {
  @ViewChild('inspectionUpsertForm') private chemigationInspectionUpsertComponent : ChemigationInspectionUpsertComponent;

  public watchUserChangeSubscription: any;
  public currentUser: UserDto;
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public annualRecords: Array<ChemigationPermitAnnualRecordDetailedDto>;

  public allYearsSelected: boolean = false;
  public yearToDisplay: number;
  public currentYear: number;
  public currentYearAnnualRecord: ChemigationPermitAnnualRecordDetailedDto;

  public inspection: ChemigationInspectionUpsertDto;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
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
    this.initializeInspectionModel(this.currentYearAnnualRecord?.ChemigationPermitAnnualRecordID);
  }

  public getInspections(): Array<ChemigationInspectionSimpleDto> {
    return this.currentYearAnnualRecord?.Inspections
      .sort((a, b) => Date.parse(b.InspectionDate) - Date.parse(a.InspectionDate))
      .sort((a, b) => a.ChemigationInspectionStatusID - b.ChemigationInspectionStatusID);
  }

  private initializeInspectionModel(annualRecordID: number) : void {
    var chemigationInspectionUpsertDto = new ChemigationInspectionUpsertDto();
    chemigationInspectionUpsertDto.ChemigationPermitAnnualRecordID = annualRecordID;
    chemigationInspectionUpsertDto.ChemigationInspectionStatusID = ChemigationInspectionStatusEnum.Pending;
    chemigationInspectionUpsertDto.ChemigationInspectionTypeID = null;
    chemigationInspectionUpsertDto.InspectionDate = null;
    chemigationInspectionUpsertDto.ChemigationInspectionFailureReasonID = null;
    chemigationInspectionUpsertDto.TillageID = null;
    chemigationInspectionUpsertDto.CropTypeID = null;
    chemigationInspectionUpsertDto.InspectorUserID = null;
    chemigationInspectionUpsertDto.ChemigationMainlineCheckValveID = null;
    chemigationInspectionUpsertDto.ChemigationLowPressureValveID = null;
    chemigationInspectionUpsertDto.ChemigationInjectionValveID = null;
    chemigationInspectionUpsertDto.HasVacuumReliefValve = false;
    chemigationInspectionUpsertDto.HasInspectionPort = false;
    chemigationInspectionUpsertDto.InspectionNotes = null;

    this.inspection = chemigationInspectionUpsertDto;
  }
}