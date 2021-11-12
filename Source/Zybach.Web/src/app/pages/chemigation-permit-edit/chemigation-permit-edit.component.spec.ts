import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitEditComponent } from './chemigation-permit-edit.component';

describe('ChemigationPermitEditComponent', () => {
  let component: ChemigationPermitEditComponent;
  let fixture: ComponentFixture<ChemigationPermitEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
