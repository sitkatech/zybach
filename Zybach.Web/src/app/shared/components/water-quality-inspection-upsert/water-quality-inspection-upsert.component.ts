import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Observable, of } from 'rxjs';
import { ChemigationInspectionService } from 'src/app/shared/generated/api/chemigation-inspection.service';
import { UserService } from 'src/app/shared/generated/api/user.service';
import { WaterQualityInspectionService } from 'src/app/shared/generated/api/water-quality-inspection.service';
import { CropTypeDto } from '../../generated/model/crop-type-dto';
import { UserDto } from '../../generated/model/user-dto';
import { WaterQualityInspectionUpsertDto } from '../../generated/model/water-quality-inspection-upsert-dto';
import { NgbDateAdapterFromString } from '../ngb-date-adapter-from-string';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { WaterQualityInspectionTypeDto } from '../../generated/model/water-quality-inspection-type-dto';
import { RoleEnum } from '../../generated/enum/role-enum';

@Component({
  selector: 'zybach-water-quality-inspection-upsert',
  templateUrl: './water-quality-inspection-upsert.component.html',
  styleUrls: ['./water-quality-inspection-upsert.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})
export class WaterQualityInspectionUpsertComponent implements OnInit {
  @Input() inspection: WaterQualityInspectionUpsertDto;
  @Output() isFormValid: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('inspectionUpsertForm',  {static: true}) public inspectionUpsertForm: NgForm;

  public inspectionTypes: Array<WaterQualityInspectionTypeDto>;
  public cropTypes: CropTypeDto[];
  public users: UserDto[];

  public searchFailed : boolean = false;

  constructor(
    private chemigationInspectionService: ChemigationInspectionService,
    private waterQualityInspectionService: WaterQualityInspectionService,
    private wellService: WellService,
    private cdr: ChangeDetectorRef,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    forkJoin({
      inspectionTypes: this.waterQualityInspectionService.waterQualityInspectionTypesGet(),
      cropTypes: this.chemigationInspectionService.cropTypesGet(),
      users: this.userService.usersNotUnassignedOrDisabledGet(),

    }).subscribe(({ inspectionTypes, cropTypes, users }) => {

      this.inspectionTypes = inspectionTypes;
      this.cropTypes = cropTypes;
      this.users = users;

      this.cdr.detectChanges();
    });
    this.validateForm();
    this.inspectionUpsertForm.valueChanges.subscribe(() => {
      this.validateForm();
    });

  }

  public validateForm(): void {
    if (this.inspectionUpsertForm.valid == true) {
        this.isFormValid.emit(true);
    } else {
        this.isFormValid.emit(false);
    }
  }

  searchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.searchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.wellsSearchWellRegistrationIDHasInspectionTypeGet(searchText) : ([])), 
        catchError(() => {
          this.searchFailed = true;
          return of([]);
        })     
      );                 
  }

}
