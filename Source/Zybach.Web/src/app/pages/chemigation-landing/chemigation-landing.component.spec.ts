import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ChemigationLandingComponent } from './chemigation-landing.component';

describe('ChemigationLandingComponent', () => {
  let component: ChemigationLandingComponent;
  let fixture: ComponentFixture<ChemigationLandingComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemigationLandingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
