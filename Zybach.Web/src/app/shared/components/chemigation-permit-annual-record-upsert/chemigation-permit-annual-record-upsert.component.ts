import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { ChemigationPermitService } from 'src/app/shared/generated/api/chemigation-permit.service';
import { ChemigationInjectionUnitTypeDto } from '../../generated/model/chemigation-injection-unit-type-dto';
import { ChemigationPermitAnnualRecordStatusDto } from '../../generated/model/chemigation-permit-annual-record-status-dto';
import { ChemigationPermitAnnualRecordUpsertDto } from '../../generated/model/chemigation-permit-annual-record-upsert-dto';
import { NgbDateAdapterFromString } from '../ngb-date-adapter-from-string';
import { States } from '../../models/enums/states.enum';
import { ChemigationPermitAnnualRecordFeeTypeDto } from '../../generated/model/chemigation-permit-annual-record-fee-type-dto';
import { ChemigationPermitAnnualRecordService } from '../../generated/api/chemigation-permit-annual-record.service';

@Component({
  selector: 'zybach-chemigation-permit-annual-record-upsert',
  templateUrl: './chemigation-permit-annual-record-upsert.component.html',
  styleUrls: ['./chemigation-permit-annual-record-upsert.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})

export class ChemigationPermitAnnualRecordUpsertComponent implements OnInit {
  @Input() model: ChemigationPermitAnnualRecordUpsertDto;
  @ViewChild('annualRecordForm',  {static: true}) public annualRecordForm: NgForm;
  
  public injectionUnitTypes: Array<ChemigationInjectionUnitTypeDto>;
  public annualRecordStatuses: Array<ChemigationPermitAnnualRecordStatusDto>;
  public feeTypes: Array<ChemigationPermitAnnualRecordFeeTypeDto>;
  public states: Object;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private chemigationPermitAnnualRecordService: ChemigationPermitAnnualRecordService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      annualRecordStatuses: this.chemigationPermitAnnualRecordService.chemigationPermitAnnualRecordStatusesGet(),
      injectionUnitTypes: this.chemigationPermitAnnualRecordService.chemigationInjectionUnitTypesGet(),
      feeTypes: this.chemigationPermitAnnualRecordService.chemigationPermitAnnualRecordFeeTypesGet()
    }).subscribe(({ annualRecordStatuses, injectionUnitTypes, feeTypes }) => {
      this.annualRecordStatuses = annualRecordStatuses;
      this.injectionUnitTypes = injectionUnitTypes;
      this.feeTypes = feeTypes;
      this.cdr.detectChanges();
    });
    this.states = States.statesList;
  }
}
