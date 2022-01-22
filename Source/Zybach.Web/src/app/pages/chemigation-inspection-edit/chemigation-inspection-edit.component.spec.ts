import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationInspectionEditComponent } from './chemigation-inspection-edit.component';

describe('ChemigationInspectionEditComponent', () => {
  let component: ChemigationInspectionEditComponent;
  let fixture: ComponentFixture<ChemigationInspectionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationInspectionEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationInspectionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
