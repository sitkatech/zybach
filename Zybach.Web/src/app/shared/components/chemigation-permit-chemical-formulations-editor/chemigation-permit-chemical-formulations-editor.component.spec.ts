import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitChemicalFormulationsEditorComponent } from './chemigation-permit-chemical-formulations-editor.component';

describe('ChemigationPermitChemicalFormulationsEditorComponent', () => {
  let component: ChemigationPermitChemicalFormulationsEditorComponent;
  let fixture: ComponentFixture<ChemigationPermitChemicalFormulationsEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitChemicalFormulationsEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitChemicalFormulationsEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
