import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-priority-event',
  templateUrl: './ticket-priority-event.component.html',
  styleUrls: ['./ticket-priority-event.component.css']
})
export class TicketPriorityEventComponent extends TicketEventComponent {
  constructor() {
    super();
    this.icon = ['fa', 'fa-list-ol'];
 }
}
