import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatershedDetailComponent } from './watershed-detail.component';

describe('WatershedDetailComponent', () => {
  let component: WatershedDetailComponent;
  let fixture: ComponentFixture<WatershedDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatershedDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatershedDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
