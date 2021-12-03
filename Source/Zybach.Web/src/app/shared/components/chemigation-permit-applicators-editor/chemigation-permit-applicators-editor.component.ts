import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationPermitAnnualRecordApplicatorUpsertDto } from '../../generated/model/chemigation-permit-annual-record-applicator-upsert-dto';

@Component({
  selector: 'zybach-chemigation-permit-applicators-editor',
  templateUrl: './chemigation-permit-applicators-editor.component.html',
  styleUrls: ['./chemigation-permit-applicators-editor.component.scss']
})
export class ChemigationPermitApplicatorsEditorComponent implements OnInit {
  @Input() model: ChemigationPermitAnnualRecordApplicatorUpsertDto[];
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('applicatorsForm',  {static: true}) public applicatorsForm: NgForm;
  
  private newRecordID: number = -1;

  // TODO: make this an api call to get possible years
  public expirationYears: Array<number> = [2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034, 2035, 2036, 2037, 2038, 2039, 2040];

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.validateForm();
    this.applicatorsForm.valueChanges.subscribe(() => {
      this.validateForm();
    });
  }

  public validateForm(): void {
    if (this.applicatorsForm.valid == true) {
        this.isFormValid.emit(true);
    } else {
        this.isFormValid.emit(false);
    }
  }

  public addRow(): void{
    const newRecord = new ChemigationPermitAnnualRecordApplicatorUpsertDto();
    newRecord.ChemigationPermitAnnualRecordApplicatorID = this.newRecordID--;
    this.model.push(newRecord);
  }

  public deleteRow(row: ChemigationPermitAnnualRecordApplicatorUpsertDto): void{
    this.model.forEach( (item, index) => {
      if(item === row) this.model.splice(index,1);
    });
  }
}
