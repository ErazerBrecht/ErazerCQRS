import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { IEvent } from '../../entities/read/interfaces/iEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-event',
  templateUrl: './ticket-event.component.html',
  styleUrls: ['./ticket-event.component.css']
})
export class TicketEventComponent {
  @Input() event: IEvent;
  protected icon: Array<string>;

  constructor() { }
}
