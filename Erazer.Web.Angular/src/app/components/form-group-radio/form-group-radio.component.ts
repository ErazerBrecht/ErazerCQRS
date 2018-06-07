import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from "@angular/forms";

@Component({
  selector: 'form-group-radio',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './form-group-radio.component.html',
  styleUrls: ['./form-group-radio.component.css']
})
export class FormGroupRadioComponent {
  @Input() value;
  @Input() control: FormControl;
}
