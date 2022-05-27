import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelInspectionUpsertComponent } from './water-level-inspection-upsert.component';

describe('WaterLevelInspectionUpsertComponent', () => {
  let component: WaterLevelInspectionUpsertComponent;
  let fixture: ComponentFixture<WaterLevelInspectionUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelInspectionUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelInspectionUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
