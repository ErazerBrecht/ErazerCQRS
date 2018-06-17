import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component'
import { StatusEvent } from '../../../entities/read/events/statusEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-status-event',
  templateUrl: './ticket-status-event.component.html',
  styleUrls: ['./ticket-status-event.component.css']
})
export class TicketStatusEventComponent extends TicketEventComponent implements OnInit {
  statusEvent: StatusEvent;

  constructor() {
    super();
    this.icon = ['fa', 'fa-tasks'];
 }

 ngOnInit(): void {
  this.statusEvent = this.event as StatusEvent;
}
}
