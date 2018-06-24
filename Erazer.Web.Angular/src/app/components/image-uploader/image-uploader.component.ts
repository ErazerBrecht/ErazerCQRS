import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Subject, Observable, merge, Subscription } from 'rxjs';
import { map, scan } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

import { registerPlugin } from 'ngx-filepond';
import FilePondPluginFileValidateType from 'filepond-plugin-file-validate-type';
import FilePondPluginFileValidateSize from 'filepond-plugin-file-validate-size'
import FilePondPluginImagePreview from 'filepond-plugin-image-preview';
import FilePondPluginImageCrop from 'filepond-plugin-image-crop';

registerPlugin(FilePondPluginFileValidateType);
registerPlugin(FilePondPluginFileValidateSize);
registerPlugin(FilePondPluginFileValidateSize);
registerPlugin(FilePondPluginImageCrop);
registerPlugin(FilePondPluginImagePreview);

@Component({
  selector: 'image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.css']
})
export class ImageUploaderComponent implements OnInit, OnDestroy {
  addFileEvents$: Subject<string>;
  removeFileEvents$: Subject<string>;
  files$: Observable<Array<any>>;
  fileError$: Observable<boolean>;
  subsriptions: Array<Subscription>;

  @Input() control: FormControl
  @Output() fileChanged: EventEmitter<Array<any>> = new EventEmitter<Array<any>>();

  pondOptions = {
    class: 'my-filepond',
    multiple: true,
    labelIdle: 'Click here or drop some files',
    acceptedFileTypes: 'image/jpeg, image/png',
    maxFileSize: '4MB',
    allowImagePreview: true,
    imageCropAspectRatio: '16:9',
    imagePreviewHeight: 150,
  }

  constructor() { }

  ngOnInit() {
    this.addFileEvents$ = new Subject();
    this.removeFileEvents$ = new Subject();
    this.subsriptions = [];

    this.files$ = merge<FileActionWrapper>(
      this.addFileEvents$.pipe(map(x => new FileActionWrapper(FileActionEnum.FILE_ADDED, x))),
      this.removeFileEvents$.pipe(map(x => new FileActionWrapper(FileActionEnum.FILE_REMOVED, x)))
    ).pipe(
      scan((acc: Array<any>, v: FileActionWrapper) => {
        if (v.type === FileActionEnum.FILE_ADDED)
          return [...acc, v.file];
        else
          return acc.filter(a => a.id !== v.file.id);
      }, []),
    );

    this.fileError$ = this.files$.pipe(
      map(files => files.some(f => f.status !== 2))
    );

    this.addSubsription(this.files$.subscribe(f => this.fileChanged.emit(f)));
    this.addSubsription(this.fileError$.subscribe(e => {
      if (e)
        this.control.setErrors({ 'invalid': true });
      else
        this.control.setErrors(null);
    }));
  }

  ngOnDestroy(): void {
    this.destroySubscriptions();
  }

  pondHandleAddFile(event: any): void {
    this.addFileEvents$.next(event.file);
  }

  pondHandleRemoveFile(event: any): void {
    this.removeFileEvents$.next(event.file);
  }

  private addSubsription(subsription: Subscription): void {
    this.subsriptions.push(subsription);
  }

  private destroySubscriptions(): void {
    this.subsriptions.forEach(s => s.unsubscribe());
  }
}

// #region Helpers
/*
-------------------------------------
Helpers
Only needed in this component
-------------------------------------
*/

class FileActionWrapper {
  type: FileActionEnum;
  file: any;

  constructor(type: FileActionEnum, file: any) {
    this.type = type;
    this.file = file;
  }
}

enum FileActionEnum {
  FILE_ADDED,
  FILE_REMOVED
}
// #endregion