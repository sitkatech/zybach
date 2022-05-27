import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellRegistrationIdEditComponent } from './well-registration-id-edit.component';

describe('WellRegistrationIdEditComponent', () => {
  let component: WellRegistrationIdEditComponent;
  let fixture: ComponentFixture<WellRegistrationIdEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellRegistrationIdEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellRegistrationIdEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
