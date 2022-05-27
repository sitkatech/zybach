import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationInspectionUpsertComponent } from './chemigation-inspection-upsert.component';

describe('ChemigationInspectionUpsertComponent', () => {
  let component: ChemigationInspectionUpsertComponent;
  let fixture: ComponentFixture<ChemigationInspectionUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationInspectionUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationInspectionUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
