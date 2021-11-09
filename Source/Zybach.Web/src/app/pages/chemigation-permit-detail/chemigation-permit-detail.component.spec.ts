import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitDetailComponent } from './chemigation-permit-detail.component';

describe('ChemigationPermitDetailComponent', () => {
  let component: ChemigationPermitDetailComponent;
  let fixture: ComponentFixture<ChemigationPermitDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemigationPermitDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
