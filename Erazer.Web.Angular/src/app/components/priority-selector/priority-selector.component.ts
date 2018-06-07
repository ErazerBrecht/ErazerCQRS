import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { FormControl } from "@angular/forms";
import { PriorityValues } from '../../configuration/priorityConstants';


@Component({
  selector: 'priority-selector',
  templateUrl: './priority-selector.component.html',
  styleUrls: ['./priority-selector.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrioritySelectorComponent {
  @Input() control: FormControl;
  @Input() label: string;

  readonly priorityValues = PriorityValues; 
}
