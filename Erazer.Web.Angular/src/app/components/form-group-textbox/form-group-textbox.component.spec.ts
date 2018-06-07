import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGroupTextboxComponent } from './form-group-textbox.component';

describe('FormGroupTextboxComponent', () => {
  let component: FormGroupTextboxComponent;
  let fixture: ComponentFixture<FormGroupTextboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGroupTextboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGroupTextboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
