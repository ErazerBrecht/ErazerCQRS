import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component'

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-status-event',
  templateUrl: './ticket-status-event.component.html',
  styleUrls: ['./ticket-status-event.component.css']
})
export class TicketStatusEventComponent extends TicketEventComponent {
  constructor() {
    super();
    this.icon = ['fa', 'fa-tasks'];
 }
}
