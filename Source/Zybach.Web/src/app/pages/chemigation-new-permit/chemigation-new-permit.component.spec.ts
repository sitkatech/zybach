import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChemigationNewPermitComponent } from './chemigation-new-permit.component';

describe('ChemigationNewPermitComponent', () => {
  let component: ChemigationNewPermitComponent;
  let fixture: ComponentFixture<ChemigationNewPermitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemigationNewPermitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationNewPermitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
