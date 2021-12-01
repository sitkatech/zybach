import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitApplicatorsEditorComponent } from './chemigation-permit-applicators-editor.component';

describe('ChemigationPermitApplicatorsEditorComponent', () => {
  let component: ChemigationPermitApplicatorsEditorComponent;
  let fixture: ComponentFixture<ChemigationPermitApplicatorsEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitApplicatorsEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitApplicatorsEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
