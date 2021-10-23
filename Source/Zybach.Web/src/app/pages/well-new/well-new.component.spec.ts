import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WellNewComponent } from './well-new.component';

describe('WellNewComponent', () => {
  let component: WellNewComponent;
  let fixture: ComponentFixture<WellNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WellNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
