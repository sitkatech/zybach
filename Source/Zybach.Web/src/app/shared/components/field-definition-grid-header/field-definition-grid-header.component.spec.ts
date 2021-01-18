import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldDefinitionGridHeaderComponent } from './field-definition-grid-header.component';

describe('FieldDefinitionGridHeaderComponent', () => {
  let component: FieldDefinitionGridHeaderComponent;
  let fixture: ComponentFixture<FieldDefinitionGridHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldDefinitionGridHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldDefinitionGridHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
