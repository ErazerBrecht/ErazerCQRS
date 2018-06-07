import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'comment-box',
  templateUrl: './comment-box.component.html',
  styleUrls: ['./comment-box.component.css']
})
export class CommentBoxComponent implements OnInit {
  commentControl: FormControl;
  
  @Output() commentAdded = new EventEmitter();

  constructor() {
    this.commentControl = new FormControl('', Validators.required);
  }

  ngOnInit() {
  }

  addComment() {
    this.commentAdded.emit(this.commentControl.value);
  }

}
