import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatershedDetailPopupComponent } from './watershed-detail-popup.component';

describe('WatershedDetailPopupComponent', () => {
  let component: WatershedDetailPopupComponent;
  let fixture: ComponentFixture<WatershedDetailPopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatershedDetailPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatershedDetailPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
