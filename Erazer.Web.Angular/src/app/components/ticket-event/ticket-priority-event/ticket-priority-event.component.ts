import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component';
import { PriorityEvent } from '../../../entities/read/events/priorityEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'ticket-priority-event',
  templateUrl: './ticket-priority-event.component.html',
  styleUrls: ['./ticket-priority-event.component.css']
})
export class TicketPriorityEventComponent extends TicketEventComponent<PriorityEvent> {

  constructor() {
    super();
    this.icon = ['fa', 'fa-list-ol'];
  }
}
