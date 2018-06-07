import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from "@angular/forms";

@Component({
  selector: 'form-group-textbox',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './form-group-textbox.component.html',
  styleUrls: ['./form-group-textbox.component.css']
})
export class FormGroupTextboxComponent {
  @Input() control: FormControl;
  @Input() label: string;
  @Input() placeholder: string;
}
