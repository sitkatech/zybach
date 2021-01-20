import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WellMapComponent } from './well-map.component';

describe('WellMapComponent', () => {
  let component: WellMapComponent;
  let fixture: ComponentFixture<WellMapComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WellMapComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
