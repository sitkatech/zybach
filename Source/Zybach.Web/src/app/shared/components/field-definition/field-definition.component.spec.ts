import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldDefinitionComponent } from './field-definition.component';

describe('FieldDefinitionComponent', () => {
  let component: FieldDefinitionComponent;
  let fixture: ComponentFixture<FieldDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldDefinitionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
