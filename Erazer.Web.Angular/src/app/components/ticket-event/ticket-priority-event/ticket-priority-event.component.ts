import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component';
import { PriorityEvent } from '../../../entities/read/events/priorityEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'ticket-priority-event',
  templateUrl: './ticket-priority-event.component.html',
  styleUrls: ['./ticket-priority-event.component.css']
})
export class TicketPriorityEventComponent extends TicketEventComponent implements OnInit {
  priorityEvent: PriorityEvent;

  constructor() {
    super();
    this.icon = ['fa', 'fa-list-ol'];
  }

  ngOnInit(): void {
    this.priorityEvent = this.event as PriorityEvent;
  }
}
