import { Component, Input, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import * as Marked from 'marked';

@Component({
  selector: 'markdown-viewer',
  templateUrl: './markdown-viewer.component.html',
  styleUrls: ['./markdown-viewer.component.css']
})
export class MarkdownViewerComponent implements AfterViewInit {
  @Input() data: string
  @ViewChild('marked') div: ElementRef

  constructor() { }

  ngAfterViewInit() {
    this.div.nativeElement.innerHTML = Marked(this.data);
  }
}
