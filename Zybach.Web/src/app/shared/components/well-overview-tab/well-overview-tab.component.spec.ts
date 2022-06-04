import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellOverviewTabComponent } from './well-overview-tab.component';

describe('WellOverviewTabComponent', () => {
  let component: WellOverviewTabComponent;
  let fixture: ComponentFixture<WellOverviewTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellOverviewTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellOverviewTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
