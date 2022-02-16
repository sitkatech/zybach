import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelInspectionNewComponent } from './water-level-inspection-new.component';

describe('WaterLevelInspectionNewComponent', () => {
  let component: WaterLevelInspectionNewComponent;
  let fixture: ComponentFixture<WaterLevelInspectionNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelInspectionNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelInspectionNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
