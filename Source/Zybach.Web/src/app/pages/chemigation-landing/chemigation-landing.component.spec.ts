import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationLandingComponent } from './chemigation-landing.component';

describe('ChemigationLandingComponent', () => {
  let component: ChemigationLandingComponent;
  let fixture: ComponentFixture<ChemigationLandingComponent>;

  beforeEach(async(() => {
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
