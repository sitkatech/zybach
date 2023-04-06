import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellGroupEditComponent } from './well-group-edit.component';

describe('WellGroupEditComponent', () => {
  let component: WellGroupEditComponent;
  let fixture: ComponentFixture<WellGroupEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellGroupEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellGroupEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
