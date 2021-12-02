import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ChemigationPermitListComponent } from './chemigation-permit-list.component';

describe('ChemigationPermitListComponent', () => {
  let component: ChemigationPermitListComponent;
  let fixture: ComponentFixture<ChemigationPermitListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ChemigationPermitListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChemigationPermitListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
