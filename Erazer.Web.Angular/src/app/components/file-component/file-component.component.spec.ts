import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileComponentComponent } from './file-component.component';

describe('FileComponentComponent', () => {
  let component: FileComponentComponent;
  let fixture: ComponentFixture<FileComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
