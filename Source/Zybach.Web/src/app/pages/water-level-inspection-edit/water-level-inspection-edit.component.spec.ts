import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelInspectionEditComponent } from './water-level-inspection-edit.component';

describe('WaterLevelInspectionEditComponent', () => {
  let component: WaterLevelInspectionEditComponent;
  let fixture: ComponentFixture<WaterLevelInspectionEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelInspectionEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelInspectionEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
