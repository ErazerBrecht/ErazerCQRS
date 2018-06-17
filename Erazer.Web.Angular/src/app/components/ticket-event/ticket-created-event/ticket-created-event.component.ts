import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component';
import { IEvent } from '../../../entities/read/interfaces/iEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'ticket-created-event',
  templateUrl: './ticket-created-event.component.html',
  styleUrls: ['./ticket-created-event.component.css']
})
export class TicketCreatedEventComponent extends TicketEventComponent implements OnInit {
  createdEvent: IEvent;

  constructor() {
    super();
    this.icon = ['fa', 'fa-bookmark'];
  }

  ngOnInit(): void {
    this.createdEvent = this.event;
  }
}
