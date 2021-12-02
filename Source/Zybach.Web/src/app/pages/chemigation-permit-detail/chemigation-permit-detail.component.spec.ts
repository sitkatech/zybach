import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ChemigationPermitDetailComponent } from './chemigation-permit-detail.component';

describe('ChemigationPermitDetailComponent', () => {
  let component: ChemigationPermitDetailComponent;
  let fixture: ComponentFixture<ChemigationPermitDetailComponent>;

  beforeEach(waitForAsync(() => {
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
