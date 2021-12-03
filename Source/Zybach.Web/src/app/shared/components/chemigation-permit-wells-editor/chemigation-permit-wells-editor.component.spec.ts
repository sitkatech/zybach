import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitWellsEditorComponent } from './chemigation-permit-wells-editor.component';

describe('ChemigationPermitWellsEditorComponent', () => {
  let component: ChemigationPermitWellsEditorComponent;
  let fixture: ComponentFixture<ChemigationPermitWellsEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitWellsEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitWellsEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
