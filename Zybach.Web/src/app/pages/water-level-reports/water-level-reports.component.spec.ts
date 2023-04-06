import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelReportsComponent } from './water-level-reports.component';

describe('WaterLevelReportsComponent', () => {
  let component: WaterLevelReportsComponent;
  let fixture: ComponentFixture<WaterLevelReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelReportsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
