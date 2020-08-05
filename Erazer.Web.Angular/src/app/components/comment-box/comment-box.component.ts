import { Component, OnInit, Output, EventEmitter, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { distinctUntilChanged, map, startWith } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'comment-box',
  templateUrl: './comment-box.component.html',
  styleUrls: ['./comment-box.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentBoxComponent implements OnInit {
  public commentControl: FormControl;
  public invalid$: Observable<boolean>;

  @Output() commentAdded = new EventEmitter();

  constructor() {
    this.commentControl = new FormControl('', Validators.required);
    this.invalid$ = this.commentControl.statusChanges.pipe(map(x => x !== "VALID"), startWith(true), distinctUntilChanged());
  }

  ngOnInit() {
  }

  addComment() {
    this.commentAdded.emit(this.commentControl.value);
  }

  reset() {
    this.commentControl.patchValue('');
  }
}
