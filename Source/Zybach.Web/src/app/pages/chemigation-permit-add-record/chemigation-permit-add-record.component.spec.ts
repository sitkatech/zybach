import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitAddRecordComponent } from './chemigation-permit-add-record.component';

describe('ChemigationPermitAddRecordComponent', () => {
  let component: ChemigationPermitAddRecordComponent;
  let fixture: ComponentFixture<ChemigationPermitAddRecordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitAddRecordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitAddRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
