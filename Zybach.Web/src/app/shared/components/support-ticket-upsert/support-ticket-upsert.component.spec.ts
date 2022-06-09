import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTicketUpsertComponent } from './support-ticket-upsert.component';

describe('SupportTicketUpsertComponent', () => {
  let component: SupportTicketUpsertComponent;
  let fixture: ComponentFixture<SupportTicketUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTicketUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportTicketUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
