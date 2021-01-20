import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestAPIComponent } from './test-api.component';

describe('TestAPIComponent', () => {
  let component: TestAPIComponent;
  let fixture: ComponentFixture<TestAPIComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestAPIComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestAPIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
