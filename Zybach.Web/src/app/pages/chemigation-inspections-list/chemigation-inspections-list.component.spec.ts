import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationInspectionsListComponent } from './chemigation-inspections-list.component';

describe('ChemigationInspectionsListComponent', () => {
  let component: ChemigationInspectionsListComponent;
  let fixture: ComponentFixture<ChemigationInspectionsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationInspectionsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationInspectionsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
