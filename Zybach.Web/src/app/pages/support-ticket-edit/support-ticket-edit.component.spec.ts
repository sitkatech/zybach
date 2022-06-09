import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTicketEditComponent } from './support-ticket-edit.component';

describe('SupportTicketEditComponent', () => {
  let component: SupportTicketEditComponent;
  let fixture: ComponentFixture<SupportTicketEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTicketEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportTicketEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
