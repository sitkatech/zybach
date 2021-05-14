import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RobustReviewScenarioComponent } from './robust-review-scenario.component';

describe('RobustReviewScenarioComponent', () => {
  let component: RobustReviewScenarioComponent;
  let fixture: ComponentFixture<RobustReviewScenarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RobustReviewScenarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RobustReviewScenarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
