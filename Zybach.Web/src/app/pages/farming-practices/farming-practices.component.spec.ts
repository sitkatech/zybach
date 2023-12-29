import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FarmingPracticesComponent } from './farming-practices.component';

describe('FarmingPracticesComponent', () => {
  let component: FarmingPracticesComponent;
  let fixture: ComponentFixture<FarmingPracticesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FarmingPracticesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FarmingPracticesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
