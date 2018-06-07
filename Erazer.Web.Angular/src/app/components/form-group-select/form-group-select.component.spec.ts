import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGroupSelectComponent } from './form-group-select.component';

describe('FormGroupSelectComponent', () => {
  let component: FormGroupSelectComponent;
  let fixture: ComponentFixture<FormGroupSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGroupSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGroupSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
