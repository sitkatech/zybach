import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellGroupWaterLevelsTabComponent } from './well-group-water-levels-tab.component';

describe('WellGroupWaterLevelsTabComponent', () => {
  let component: WellGroupWaterLevelsTabComponent;
  let fixture: ComponentFixture<WellGroupWaterLevelsTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellGroupWaterLevelsTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellGroupWaterLevelsTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
