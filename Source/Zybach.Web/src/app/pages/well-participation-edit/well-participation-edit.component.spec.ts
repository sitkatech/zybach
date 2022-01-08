import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellParticipationEditComponent } from './well-participation-edit.component';

describe('WellParticipationEditComponent', () => {
  let component: WellParticipationEditComponent;
  let fixture: ComponentFixture<WellParticipationEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellParticipationEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellParticipationEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
