import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { ChemigationInspectionService } from 'src/app/services/chemigation-inspection.service';
import { UserService } from 'src/app/services/user/user.service';
import { WaterQualityInspectionService } from 'src/app/services/water-quality-inspection.service';
import { CropTypeDto } from '../../generated/model/crop-type-dto';
import { UserDto } from '../../generated/model/user-dto';
import { WaterQualityInspectionUpsertDto } from '../../generated/model/water-quality-inspection-upsert-dto';
import { NgbDateAdapterFromString } from '../ngb-date-adapter-from-string';

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

  public cropTypes: CropTypeDto[];
  public users: UserDto[];

  constructor(
    private chemigationInspectionService: ChemigationInspectionService,
    private waterQualityInspectionService: WaterQualityInspectionService,
    private cdr: ChangeDetectorRef,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    forkJoin({
      cropTypes: this.chemigationInspectionService.getCropTypes(),
      users: this.userService.getUsers(),

    }).subscribe(({ cropTypes, users }) => {

      this.cropTypes = cropTypes;
      this.users = users;

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
