import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationPermitUpsertComponent } from './chemigation-permit-upsert.component';

describe('ChemigationPermitUpsertComponent', () => {
  let component: ChemigationPermitUpsertComponent;
  let fixture: ComponentFixture<ChemigationPermitUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChemigationPermitUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
