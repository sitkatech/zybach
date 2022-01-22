import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelInspectionListComponent } from './water-level-inspection-list.component';

describe('WaterLevelInspectionListComponent', () => {
  let component: WaterLevelInspectionListComponent;
  let fixture: ComponentFixture<WaterLevelInspectionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelInspectionListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelInspectionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
