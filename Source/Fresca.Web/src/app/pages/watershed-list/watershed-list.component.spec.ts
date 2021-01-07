import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatershedListComponent } from './watershed-list.component';

describe('WatershedListComponent', () => {
  let component: WatershedListComponent;
  let fixture: ComponentFixture<WatershedListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatershedListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatershedListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
