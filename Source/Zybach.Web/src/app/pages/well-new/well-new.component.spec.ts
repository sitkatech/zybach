import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WellNewComponent } from './well-new.component';

describe('WellNewComponent', () => {
  let component: WellNewComponent;
  let fixture: ComponentFixture<WellNewComponent>;

  beforeEach(waitForAsync(() => {
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
