import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitEditRecordComponent } from './chemigation-permit-edit-record.component';

describe('ChemigationPermitEditRecordComponent', () => {
  let component: ChemigationPermitEditRecordComponent;
  let fixture: ComponentFixture<ChemigationPermitEditRecordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitEditRecordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitEditRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
