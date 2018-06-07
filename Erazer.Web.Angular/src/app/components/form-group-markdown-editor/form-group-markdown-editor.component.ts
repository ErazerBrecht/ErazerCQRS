import { Component, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { FormControl } from "@angular/forms";
import * as SimpleMDE from 'simplemde';

@Component({
  selector: 'form-group-markdown-editor',
  templateUrl: './form-group-markdown-editor.component.html',
  styleUrls: ['./form-group-markdown-editor.component.css']
})
export class FormGroupMarkdownEditorComponent implements AfterViewInit {
  @Input() label: string
  @Input() control: FormControl
  @Input() previewMode: boolean;
  @ViewChild('simplemde') textarea: ElementRef
  private simplemde: SimpleMDE;

  constructor() { }

  ngAfterViewInit() {
    this.simplemde = new SimpleMDE({ element: this.textarea.nativeElement, spellChecker: false });

    if (this.previewMode) {
      this.simplemde.togglePreview();
    }

    this.simplemde.codemirror.on('change', () => {
      this.control.patchValue(this.simplemde.value());
    })
  }


}
