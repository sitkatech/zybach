import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTicketCommentNewComponent } from './support-ticket-comment-new.component';

describe('SupportTicketCommentNewComponent', () => {
  let component: SupportTicketCommentNewComponent;
  let fixture: ComponentFixture<SupportTicketCommentNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTicketCommentNewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportTicketCommentNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
