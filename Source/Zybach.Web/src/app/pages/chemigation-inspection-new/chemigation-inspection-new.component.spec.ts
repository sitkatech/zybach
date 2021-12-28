import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationInspectionNewComponent } from './chemigation-inspection-new.component';

describe('ChemigationInspectionNewComponent', () => {
  let component: ChemigationInspectionNewComponent;
  let fixture: ComponentFixture<ChemigationInspectionNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationInspectionNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationInspectionNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
