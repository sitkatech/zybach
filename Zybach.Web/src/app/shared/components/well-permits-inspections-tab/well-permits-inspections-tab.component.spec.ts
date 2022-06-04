import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WellPermitsInspectionsTabComponent } from './well-permits-inspections-tab.component';

describe('WellPermitsInspectionsTabComponent', () => {
  let component: WellPermitsInspectionsTabComponent;
  let fixture: ComponentFixture<WellPermitsInspectionsTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WellPermitsInspectionsTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WellPermitsInspectionsTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
