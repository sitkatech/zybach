import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { LinkRendererComponent } from './link-renderer.component';

describe('LinkRendererComponent', () => {
  let component: LinkRendererComponent;
  let fixture: ComponentFixture<LinkRendererComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkRendererComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkRendererComponent);
    component = fixture.componentInstance;
    component.params = {
      value: {}
    }
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
