import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ChemigationNewPermitComponent } from './chemigation-new-permit.component';

describe('ChemigationNewPermitComponent', () => {
  let component: ChemigationNewPermitComponent;
  let fixture: ComponentFixture<ChemigationNewPermitComponent>;

  beforeEach(waitForAsync(() => {
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
