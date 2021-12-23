import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { UserService } from 'src/app/services/user/user.service';
import { ChemigationInjectionValveDto } from '../../generated/model/chemigation-injection-valve-dto';
import { ChemigationInspectionFailureReasonDto } from '../../generated/model/chemigation-inspection-failure-reason-dto';
import { ChemigationInspectionStatusDto } from '../../generated/model/chemigation-inspection-status-dto';
import { ChemigationInspectionTypeDto } from '../../generated/model/chemigation-inspection-type-dto';
import { ChemigationInspectionUpsertDto } from '../../generated/model/chemigation-inspection-upsert-dto';
import { ChemigationLowPressureValveDto } from '../../generated/model/chemigation-low-pressure-valve-dto';
import { ChemigationMainlineCheckValveDto } from '../../generated/model/chemigation-mainline-check-valve-dto';
import { CropTypeDto } from '../../generated/model/crop-type-dto';
import { TillageDto } from '../../generated/model/tillage-dto';
import { UserDto } from '../../generated/model/user-dto';

@Component({
  selector: 'zybach-chemigation-inspection-upsert',
  templateUrl: './chemigation-inspection-upsert.component.html',
  styleUrls: ['./chemigation-inspection-upsert.component.scss']
})
export class ChemigationInspectionUpsertComponent implements OnInit {
  @Input() inspection: ChemigationInspectionUpsertDto;
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('inspectionUpsertForm',  {static: true}) public inspectionUpsertForm: NgForm;

  public chemigationInspectionTypes: ChemigationInspectionTypeDto[];
  public chemigationInspectionStatuses: ChemigationInspectionStatusDto[];
  public chemigationInspectionFailureReasons: ChemigationInspectionFailureReasonDto[];
  public tillages: TillageDto[];
  public cropTypes: CropTypeDto[];
  public users: UserDto[];
  public chemigationMainlineCheckValves: ChemigationMainlineCheckValveDto[];
  public chemigationLowPressureValves: ChemigationLowPressureValveDto[];
  public chemigationInjectionValves: ChemigationInjectionValveDto[];

  
  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private chemigationInspectionService: ChemigationInspectionService,
    private cdr: ChangeDetectorRef,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    forkJoin({
      chemigationInspectionTypes: this.chemigationInspectionService.getChemigationInspectionTypes(),
      chemigationInspectionStatuses: this.chemigationInspectionService.getChemigationInspectionStatuses(),
      chemigationInspectionFailureReasons: this.chemigationInspectionService.getChemigationInspectionFailureReasons(),
      tillages: this.chemigationInspectionService.getTillageTypes(),
      cropTypes: this.chemigationInspectionService.getCropTypes(),
      users: this.userService.getUsers(),
      chemigationMainlineCheckValves: this.chemigationInspectionService.getMainlineCheckValves(),
      chemigationLowPressureValves: this.chemigationInspectionService.getLowPressureValves(),
      chemigationInjectionValves: this.chemigationInspectionService.getChemigationInjectionValves()
    }).subscribe(({ chemigationInspectionTypes, chemigationInspectionStatuses,
      chemigationInspectionFailureReasons, tillages, cropTypes, users,
      chemigationMainlineCheckValves, chemigationLowPressureValves, chemigationInjectionValves}) => {
      this.chemigationInspectionTypes = chemigationInspectionTypes;
      this.chemigationInspectionStatuses = chemigationInspectionStatuses;
      this.chemigationInspectionFailureReasons = chemigationInspectionFailureReasons;
      this.tillages = tillages;
      this.cropTypes = cropTypes;
      this.users = users;
      this.chemigationMainlineCheckValves = chemigationMainlineCheckValves;
      this.chemigationLowPressureValves = chemigationLowPressureValves;
      this.chemigationInjectionValves = chemigationInjectionValves;
      this.cdr.detectChanges();
    });
    this.validateForm();

  }

  public validateForm(): void {
    if (this.inspectionUpsertForm.valid == true) {
        this.isFormValid.emit(true);
    } else {
        this.isFormValid.emit(false);
    }
  }

}
