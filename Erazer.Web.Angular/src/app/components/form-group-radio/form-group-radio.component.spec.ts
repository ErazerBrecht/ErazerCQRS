import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGroupRadioComponent } from './form-group-radio.component';

describe('FormGroupRadioComponent', () => {
  let component: FormGroupRadioComponent;
  let fixture: ComponentFixture<FormGroupRadioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGroupRadioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGroupRadioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
