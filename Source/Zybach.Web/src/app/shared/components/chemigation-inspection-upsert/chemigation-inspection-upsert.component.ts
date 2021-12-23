import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ChemigationPermitService } from 'src/app/services/chemigation-permit.service';
import { ChemigationInspectionUpsertDto } from '../../generated/model/chemigation-inspection-upsert-dto';

@Component({
  selector: 'zybach-chemigation-inspection-upsert',
  templateUrl: './chemigation-inspection-upsert.component.html',
  styleUrls: ['./chemigation-inspection-upsert.component.scss']
})
export class ChemigationInspectionUpsertComponent implements OnInit {
  @Input() model: ChemigationInspectionUpsertDto;
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('inspectionUpsertForm',  {static: true}) public inspectionUpsertForm: NgForm;
  
  constructor(
    private chemigationPermitService: ChemigationPermitService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    forkJoin({
      // chemicalFormulations: this.chemigationPermitService.getChemicalFormulations(),
      // chemicalUnits: this.chemigationPermitService.getChemicalUnits()
    }).subscribe(({ chemicalFormulations, chemicalUnits }) => {
      // this.chemicalFormulations = chemicalFormulations;
      // this.chemicalUnits = chemicalUnits;
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
