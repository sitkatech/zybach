import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContextMenuRendererComponent } from './context-menu-renderer.component';

describe('ContextMenuRendererComponent', () => {
  let component: ContextMenuRendererComponent;
  let fixture: ComponentFixture<ContextMenuRendererComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContextMenuRendererComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContextMenuRendererComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
