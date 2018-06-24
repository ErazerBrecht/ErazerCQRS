import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component'
import { StatusEvent } from '../../../entities/read/events/statusEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-status-event',
  templateUrl: './ticket-status-event.component.html',
  styleUrls: ['./ticket-status-event.component.css']
})
export class TicketStatusEventComponent extends TicketEventComponent<StatusEvent> {

  constructor() {
    super();
    this.icon = ['fa', 'fa-tasks'];
 }
}
