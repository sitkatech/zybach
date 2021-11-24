import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
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
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('annualRecordForm',  {static: true}) public annualRecordForm: NgForm;
  
  public injectionUnitTypes: Array<ChemigationInjectionUnitTypeDto>;
  public annualRecordStatuses: Array<ChemigationPermitAnnualRecordStatusDto>;

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
    this.populateDates();
    this.validateForm();
    this.annualRecordForm.valueChanges.subscribe(() => {
      this.validateForm();
    });
  }

  public validateForm(): void {
    if (this.annualRecordForm.valid == true) {
        this.isFormValid.emit(true);
    } else {
        this.isFormValid.emit(false);
    }
  }

  private populateDates(): void {
    if (this.model.DatePaid != null) {
      this.model.DatePaid = new Date(this.model.DatePaid);
    }
    if (this.model.DateReceived != null) {
      this.model.DateReceived = new Date(this.model.DateReceived);
    }
  }

}
