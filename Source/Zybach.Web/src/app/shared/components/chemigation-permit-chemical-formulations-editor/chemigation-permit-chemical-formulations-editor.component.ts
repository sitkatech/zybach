import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemicalFormulationDto } from '../../generated/model/chemical-formulation-dto';
import { ChemicalUnitDto } from '../../generated/model/chemical-unit-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationSimpleDto } from '../../generated/model/chemigation-permit-annual-record-chemical-formulation-simple-dto';

@Component({
  selector: 'zybach-chemigation-permit-chemical-formulations-editor',
  templateUrl: './chemigation-permit-chemical-formulations-editor.component.html',
  styleUrls: ['./chemigation-permit-chemical-formulations-editor.component.scss']
})
export class ChemigationPermitChemicalFormulationsEditorComponent implements OnInit {
  @Input() model: ChemigationPermitAnnualRecordChemicalFormulationSimpleDto[];
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('chemicalFormulationsForm',  {static: true}) public annualRecordForm: NgForm;
  
  public chemicalUnits: Array<ChemicalUnitDto>;
  public chemicalFormulations: Array<ChemicalFormulationDto>;
  private newRecordID: number = -1;

  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      chemicalFormulations: this.chemigationPermitService.getChemicalFormulations(),
      chemicalUnits: this.chemigationPermitService.getChemicalUnits()
    }).subscribe(({ chemicalFormulations, chemicalUnits }) => {
      this.chemicalFormulations = chemicalFormulations;
      this.chemicalUnits = chemicalUnits;
      this.cdr.detectChanges();
    });
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

  public addRow(): void{
    const newRecord = new ChemigationPermitAnnualRecordChemicalFormulationSimpleDto();
    newRecord.ChemigationPermitAnnualRecordChemicalFormulationID = this.newRecordID--;
    this.model.push(newRecord);
  }

  public deleteRow(row: ChemigationPermitAnnualRecordChemicalFormulationSimpleDto): void{
    this.model.forEach( (item, index) => {
      if(item === row) this.model.splice(index,1);
    });
  }
}
