import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ChemigationPermitAnnualRecordChemicalFormulationService } from '../../generated/api/chemigation-permit-annual-record-chemical-formulation.service';
import { ChemicalFormulationDto } from '../../generated/model/chemical-formulation-dto';
import { ChemicalUnitDto } from '../../generated/model/chemical-unit-dto';
import { ChemigationPermitAnnualRecordChemicalFormulationUpsertDto } from '../../generated/model/chemigation-permit-annual-record-chemical-formulation-upsert-dto';

@Component({
  selector: 'zybach-chemigation-permit-chemical-formulations-editor',
  templateUrl: './chemigation-permit-chemical-formulations-editor.component.html',
  styleUrls: ['./chemigation-permit-chemical-formulations-editor.component.scss']
})
export class ChemigationPermitChemicalFormulationsEditorComponent implements OnInit {
  @Input() model: ChemigationPermitAnnualRecordChemicalFormulationUpsertDto[];
  @ViewChild('chemicalFormulationsForm',  {static: true}) public chemicalFormulationsForm: NgForm;
  
  public chemicalUnits: Array<ChemicalUnitDto>;
  public chemicalFormulations: Array<ChemicalFormulationDto>;
  private newRecordID: number = -1;

  constructor(
    private chemigationPermitAnnualRecordChemicalFormulationService: ChemigationPermitAnnualRecordChemicalFormulationService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      chemicalFormulations: this.chemigationPermitAnnualRecordChemicalFormulationService.chemicalFormulationsGet(),
      chemicalUnits: this.chemigationPermitAnnualRecordChemicalFormulationService.chemicalUnitsGet()
    }).subscribe(({ chemicalFormulations, chemicalUnits }) => {
      this.chemicalFormulations = chemicalFormulations;
      this.chemicalUnits = chemicalUnits;
      this.cdr.detectChanges();
    });
  }

  public addRow(): void{
    const newRecord = new ChemigationPermitAnnualRecordChemicalFormulationUpsertDto();
    newRecord.ChemigationPermitAnnualRecordChemicalFormulationID = this.newRecordID--;
    this.model.push(newRecord);
  }

  public deleteRow(row: ChemigationPermitAnnualRecordChemicalFormulationUpsertDto): void{
    this.model.forEach( (item, index) => {
      if(item === row) this.model.splice(index,1);
    });
  }
}
