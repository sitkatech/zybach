import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgbDate, NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordUpsertDto } from '../../models/chemigation-permit-annual-record-upsert-dto';
import { ChemigationInjectionUnitTypeDto } from '../../models/generated/chemigation-injection-unit-type-dto';
import { ChemigationPermitAnnualRecordStatusDto } from '../../models/generated/chemigation-permit-annual-record-status-dto';

@Component({
  selector: 'zybach-chemigation-permit-annual-record-upsert',
  templateUrl: './chemigation-permit-annual-record-upsert.component.html',
  styleUrls: ['./chemigation-permit-annual-record-upsert.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeAdapter}]
})

export class ChemigationPermitAnnualRecordUpsertComponent implements OnInit {

  @Input() model: ChemigationPermitAnnualRecordUpsertDto;

  public injectionUnitTypes: Array<ChemigationInjectionUnitTypeDto>;
  public annualRecordStatuses: Array<ChemigationPermitAnnualRecordStatusDto>;

  @Output() isCPARUpsertFormValid: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('cparUpsert') form;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      annualRecordStatuses: this.chemigationPermitService.getAnnualRecordStatusTypes(),
      injectionUnitTypes: this.chemigationPermitService.getAllChemigationInjectionUnitTypes()
    }).subscribe(({ annualRecordStatuses, injectionUnitTypes }) => {
      this.annualRecordStatuses = annualRecordStatuses;
      this.injectionUnitTypes = injectionUnitTypes;
      this.cdr.detectChanges();
    });
    this.model.DatePaid = new Date(this.model.DatePaid);
    this.model.DateReceived = new Date(this.model.DateReceived);
    this.form?.patchValue(this.model);
    this.validateCPARForm();
  }
  
  public validateCPARForm(): void {
    if(this.form?.valid == true) {
        this.isCPARUpsertFormValid.emit(true);
    } else {
        this.isCPARUpsertFormValid.emit(false);
    }
  }
}
