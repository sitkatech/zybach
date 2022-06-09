import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportTicketListComponent } from './support-ticket-list.component';

describe('SupportTicketListComponent', () => {
  let component: SupportTicketListComponent;
  let fixture: ComponentFixture<SupportTicketListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportTicketListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportTicketListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
