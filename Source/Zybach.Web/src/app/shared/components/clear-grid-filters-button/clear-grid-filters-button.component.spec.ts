import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClearGridFiltersButtonComponent } from './clear-grid-filters-button.component';

describe('ClearGridFiltersButtonComponent', () => {
  let component: ClearGridFiltersButtonComponent;
  let fixture: ComponentFixture<ClearGridFiltersButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClearGridFiltersButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClearGridFiltersButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
