import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmingPracticesMapComponent } from './farming-practices-map.component';

describe('FarmingPracticesMapComponent', () => {
  let component: FarmingPracticesMapComponent;
  let fixture: ComponentFixture<FarmingPracticesMapComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FarmingPracticesMapComponent]
    });
    fixture = TestBed.createComponent(FarmingPracticesMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
