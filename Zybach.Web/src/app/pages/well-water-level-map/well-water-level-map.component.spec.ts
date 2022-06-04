import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellWaterLevelMapComponent } from './well-water-level-map.component';

describe('WellWaterLevelMapComponent', () => {
  let component: WellWaterLevelMapComponent;
  let fixture: ComponentFixture<WellWaterLevelMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellWaterLevelMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellWaterLevelMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
