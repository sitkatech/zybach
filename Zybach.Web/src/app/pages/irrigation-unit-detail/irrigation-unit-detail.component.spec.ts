import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IrrigationUnitDetailComponent } from './irrigation-unit-detail.component';

describe('IrrigationUnitDetailComponent', () => {
  let component: IrrigationUnitDetailComponent;
  let fixture: ComponentFixture<IrrigationUnitDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IrrigationUnitDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IrrigationUnitDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
