import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NeighborhoodExplorerComponent } from './neighborhood-explorer.component';

describe('NeighborhoodExplorerComponent', () => {
  let component: NeighborhoodExplorerComponent;
  let fixture: ComponentFixture<NeighborhoodExplorerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NeighborhoodExplorerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NeighborhoodExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
