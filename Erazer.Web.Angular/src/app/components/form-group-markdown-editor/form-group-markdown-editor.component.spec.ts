import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormGroupMarkdownEditorComponent } from './form-group-markdown-editor.component';

describe('FormGroupMarkdownEditorComponent', () => {
  let component: FormGroupMarkdownEditorComponent;
  let fixture: ComponentFixture<FormGroupMarkdownEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGroupMarkdownEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGroupMarkdownEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
