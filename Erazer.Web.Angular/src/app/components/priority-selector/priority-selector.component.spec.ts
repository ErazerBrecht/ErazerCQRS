import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrioritySelectorComponent } from './priority-selector.component';

describe('PrioritySelectorComponent', () => {
  let component: PrioritySelectorComponent;
  let fixture: ComponentFixture<PrioritySelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrioritySelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrioritySelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
