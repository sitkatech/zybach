import { ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, Observable, of } from 'rxjs';
import { UserService } from 'src/app/services/user/user.service';
import { CropTypeDto } from '../../generated/model/crop-type-dto';
import { UserDto } from '../../generated/model/user-dto';
import { NgbDateAdapterFromString } from '../ngb-date-adapter-from-string';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { WellService } from 'src/app/services/well.service';
import { WaterLevelInspectionUpsertDto } from '../../generated/model/water-level-inspection-upsert-dto';


@Component({
  selector: 'zybach-water-level-inspection-upsert',
  templateUrl: './water-level-inspection-upsert.component.html',
  styleUrls: ['./water-level-inspection-upsert.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateAdapterFromString}]
})
export class WaterLevelInspectionUpsertComponent implements OnInit {
  @Input() inspection: WaterLevelInspectionUpsertDto;
  @ViewChild('inspectionUpsertForm',  {static: true}) public inspectionUpsertForm: NgForm;

  public users: UserDto[];

  public searchFailed : boolean = false;

  constructor(
    private wellService: WellService,
    private cdr: ChangeDetectorRef,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    forkJoin({
      users: this.userService.getUsers(),

    }).subscribe(({ users }) => {

      this.users = users;

      this.cdr.detectChanges();
    });
  }

  searchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.searchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.searchByWellRegistrationIDHasInspectionType(searchText) : ([])), 
        catchError(() => {
          this.searchFailed = true;
          return of([]);
        })     
      );                 
  }

}

