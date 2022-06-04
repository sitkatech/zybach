import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitReportsComponent } from './chemigation-permit-reports.component';

describe('ChemigationPermitReportsComponent', () => {
  let component: ChemigationPermitReportsComponent;
  let fixture: ComponentFixture<ChemigationPermitReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
