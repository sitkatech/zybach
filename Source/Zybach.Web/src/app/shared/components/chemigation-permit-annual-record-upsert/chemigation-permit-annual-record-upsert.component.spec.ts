import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitAnnualRecordUpsertComponent } from './chemigation-permit-annual-record-upsert.component';

describe('ChemigationPermitAnnualRecordUpsertComponent', () => {
  let component: ChemigationPermitAnnualRecordUpsertComponent;
  let fixture: ComponentFixture<ChemigationPermitAnnualRecordUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitAnnualRecordUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitAnnualRecordUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
