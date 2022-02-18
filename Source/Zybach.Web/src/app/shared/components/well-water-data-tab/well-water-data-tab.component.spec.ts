import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellWaterDataTabComponent } from './well-water-data-tab.component';

describe('WellWaterDataTabComponent', () => {
  let component: WellWaterDataTabComponent;
  let fixture: ComponentFixture<WellWaterDataTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellWaterDataTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellWaterDataTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
