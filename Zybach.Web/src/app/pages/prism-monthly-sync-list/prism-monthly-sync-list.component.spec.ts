import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrismMonthlySyncListComponent } from './prism-monthly-sync-list.component';

describe('PrismMonthlySyncListComponent', () => {
  let component: PrismMonthlySyncListComponent;
  let fixture: ComponentFixture<PrismMonthlySyncListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PrismMonthlySyncListComponent]
    });
    fixture = TestBed.createComponent(PrismMonthlySyncListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
