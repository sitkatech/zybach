import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WaterLevelExplorerComponent } from './water-level-explorer.component';

describe('WaterLevelExplorerComponent', () => {
  let component: WaterLevelExplorerComponent;
  let fixture: ComponentFixture<WaterLevelExplorerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WaterLevelExplorerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WaterLevelExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
