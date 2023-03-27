import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellGroupListComponent } from './well-group-list.component';

describe('WellGroupListComponent', () => {
  let component: WellGroupListComponent;
  let fixture: ComponentFixture<WellGroupListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellGroupListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellGroupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
