import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellGroupDetailComponent } from './well-group-detail.component';

describe('WellGroupDetailComponent', () => {
  let component: WellGroupDetailComponent;
  let fixture: ComponentFixture<WellGroupDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellGroupDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellGroupDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
