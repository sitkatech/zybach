import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ChemigationPermitService } from 'src/app/shared/generated/api/chemigation-permit.service';
import { WellService } from 'src/app/shared/generated/api/well.service';
import { ChemigationPermitDto } from 'src/app/shared/generated/model/chemigation-permit-dto';
import { ChemigationPermitStatusDto } from 'src/app/shared/generated/model/chemigation-permit-status-dto';
import { ChemigationPermitUpsertDto } from 'src/app/shared/generated/model/chemigation-permit-upsert-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { AlertService } from 'src/app/shared/services/alert.service';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap, switchMap, catchError } from 'rxjs/operators';
import { CountyDto } from 'src/app/shared/generated/model/county-dto';
import { CountyService } from 'src/app/shared/generated/api/county.service';

@Component({
  selector: 'zybach-chemigation-permit-edit',
  templateUrl: './chemigation-permit-edit.component.html',
  styleUrls: ['./chemigation-permit-edit.component.scss']
})
export class ChemigationPermitEditComponent implements OnInit, OnDestroy {
  private currentUser: UserDto;
  
  public chemigationPermitNumber: number;
  public chemigationPermit: ChemigationPermitDto;
  public permitStatuses: Array<ChemigationPermitStatusDto>;
  public counties: Array<CountyDto>;
  public model: ChemigationPermitUpsertDto;
  
  public isLoadingSubmit: boolean = false;
  public searchFailed : boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private chemigationPermitService: ChemigationPermitService,
    private wellService: WellService,
    private countyService: CountyService,
    private cdr: ChangeDetectorRef,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.model = new ChemigationPermitUpsertDto();

    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;

      this.chemigationPermitService.chemigationPermitStatusesGet().subscribe(permitStatuses => {
        this.permitStatuses = permitStatuses;
      });

      this.countyService.countiesGet().subscribe(counties => {
        this.counties = counties;
      });

      this.chemigationPermitNumber = parseInt(this.route.snapshot.paramMap.get("permit-number"));
      this.chemigationPermitService.chemigationPermitsChemigationPermitNumberGet(this.chemigationPermitNumber).subscribe(chemigationPermit => {
        this.chemigationPermit = chemigationPermit;
        this.model.ChemigationPermitStatusID = this.chemigationPermit.ChemigationPermitStatus.ChemigationPermitStatusID;
        this.model.CountyID = this.chemigationPermit.County.CountyID;
        this.model.WellRegistrationID = this.chemigationPermit.Well?.WellRegistrationID;
        this.cdr.detectChanges();
      });

    });
  }
  
  ngOnDestroy() {
    
       this.cdr.detach();
  }

  onSubmit(editChemigationPermitForm: HTMLFormElement): void {
    this.isLoadingSubmit = true;
  
    this.chemigationPermitService.chemigationPermitsChemigationPermitIDPut(this.chemigationPermit.ChemigationPermitID, this.model)
      .subscribe(response => {
        this.isLoadingSubmit = false;
        editChemigationPermitForm.reset();
        this.router.navigateByUrl("/chemigation-permits/" + this.chemigationPermitNumber).then(() => {
          this.alertService.pushAlert(new Alert(`Chemigation Permit ${this.chemigationPermitNumber} was successfully updated.`, AlertContext.Success));
        });
      }
        ,
        error => {
          this.isLoadingSubmit = false;
          this.cdr.detectChanges();
        }
      );
  }

  searchApi = (text$: Observable<string>) => {
    return text$.pipe(      
        debounceTime(200), 
        distinctUntilChanged(),
        tap(() => this.searchFailed = false),
        switchMap(searchText => searchText.length > 2 ? this.wellService.wellsSearchWellRegistrationIDRequiresChemigationGet(searchText) : ([])), 
        catchError(() => {
          this.searchFailed = true;
          return of([]);
        })     
      );                 
  }
}