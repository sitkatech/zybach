import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTicketNewComponent } from './support-ticket-new.component';

describe('SupportTicketNewComponent', () => {
  let component: SupportTicketNewComponent;
  let fixture: ComponentFixture<SupportTicketNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTicketNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportTicketNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
