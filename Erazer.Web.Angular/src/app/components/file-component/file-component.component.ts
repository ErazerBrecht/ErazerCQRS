import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { DOCUMENT_API } from '../../configuration/config';
import { File } from '../../entities/read/file';

@Component({
  selector: 'file-component',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './file-component.component.html',
  styleUrls: ['./file-component.component.css']
})
export class FileComponentComponent implements OnInit {
 
  @Input() file: File;
  url: string;

  constructor() { }

  ngOnInit() {
    this.url = `${DOCUMENT_API}/api/file/${this.file.id}`;
  }

}
