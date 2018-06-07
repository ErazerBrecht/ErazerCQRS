import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from "@angular/forms";
import { IOption } from '../../configuration/ioption';

@Component({
  selector: 'form-group-select',
  templateUrl: './form-group-select.component.html',
  styleUrls: ['./form-group-select.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormGroupSelectComponent {
  @Input() label: string;
  @Input() placeholder: string;
  @Input() options: Array<IOption>;
  @Input() control: FormControl;
}
