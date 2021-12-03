import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { forkJoin, Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { WellService } from 'src/app/services/well.service';
import { ChemigationPermitAnnualRecordWellUpsertDto } from '../../generated/model/chemigation-permit-annual-record-well-upsert-dto';
import { WellSimpleDto } from '../../generated/model/well-simple-dto';

@Component({
  selector: 'zybach-chemigation-permit-wells-editor',
  templateUrl: './chemigation-permit-wells-editor.component.html',
  styleUrls: ['./chemigation-permit-wells-editor.component.scss']
})
export class ChemigationPermitWellsEditorComponent implements OnInit {
  @Input() model: ChemigationPermitAnnualRecordWellUpsertDto[];
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('wellsForm',  {static: true}) public wellsForm: NgForm;
  
  private newRecordID: number = -1;
  public searchFailed : boolean = false;

  constructor(
    private wellService: WellService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.validateForm();
    this.wellsForm.valueChanges.subscribe(() => {
      this.validateForm();
    });
  }

  public validateForm(): void {
    if (this.wellsForm.valid == true) {
        this.isFormValid.emit(true);
    } else {
        this.isFormValid.emit(false);
    }
  }

  public addRow(): void{
    const newRecord = new ChemigationPermitAnnualRecordWellUpsertDto();
    newRecord.ChemigationPermitAnnualRecordWellID = this.newRecordID--;
    this.model.push(newRecord);
  }

  public deleteRow(row: ChemigationPermitAnnualRecordWellUpsertDto): void{
    this.model.forEach( (item, index) => {
      if(item === row) this.model.splice(index,1);
    });
  }

  searchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.searchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.searchByWellRegistrationID(searchText) : ([])), 
        catchError(() => {
          this.searchFailed = true;
          return of([]);
        })     
      );                 
  }

}