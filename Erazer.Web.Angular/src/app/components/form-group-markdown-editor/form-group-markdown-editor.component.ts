import { Component, Input, ViewChild, ElementRef, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import { FormControl } from "@angular/forms";
import * as SimpleMDE from 'easymde';
import { takeUntil, filter } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'form-group-markdown-editor',
  templateUrl: './form-group-markdown-editor.component.html',
  styleUrls: ['./form-group-markdown-editor.component.css']
})
export class FormGroupMarkdownEditorComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() label: string
  @Input() control: FormControl
  @Input() previewMode: boolean;
  @ViewChild('mde') textarea: ElementRef

  private mde: SimpleMDE;
  private destroy$ = new Subject<boolean>();

  constructor() { }

  ngOnInit() {
    this.control.valueChanges.pipe(takeUntil(this.destroy$), filter(x => x !== this.mde.value())).subscribe(x => {
      this.mde.value(x);
    });
  }

  ngAfterViewInit() {
    this.mde = new SimpleMDE({ element: this.textarea.nativeElement, spellChecker: false });

    if (this.previewMode) {
      SimpleMDE.togglePreview(this.mde);
    }

    this.mde.codemirror.on('change', () => {
      this.control.patchValue(this.mde.value());
    })
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
