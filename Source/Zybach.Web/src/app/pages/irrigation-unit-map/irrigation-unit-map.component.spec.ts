import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IrrigationUnitMapComponent } from './irrigation-unit-map.component';

describe('IrrigationUnitMapComponent', () => {
  let component: IrrigationUnitMapComponent;
  let fixture: ComponentFixture<IrrigationUnitMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IrrigationUnitMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IrrigationUnitMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
