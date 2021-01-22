import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WellExplorerComponent } from './well-explorer.component';

describe('WellExplorerComponent', () => {
  let component: WellExplorerComponent;
  let fixture: ComponentFixture<WellExplorerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WellExplorerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WellExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
