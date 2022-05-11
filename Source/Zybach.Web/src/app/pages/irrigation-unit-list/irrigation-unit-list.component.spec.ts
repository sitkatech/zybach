import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IrrigationUnitListComponent } from './irrigation-unit-list.component';

describe('IrrigationUnitListComponent', () => {
  let component: IrrigationUnitListComponent;
  let fixture: ComponentFixture<IrrigationUnitListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IrrigationUnitListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IrrigationUnitListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
