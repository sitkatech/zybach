import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellContactEditComponent } from './well-contact-edit.component';

describe('WellContactEditComponent', () => {
  let component: WellContactEditComponent;
  let fixture: ComponentFixture<WellContactEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellContactEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellContactEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
