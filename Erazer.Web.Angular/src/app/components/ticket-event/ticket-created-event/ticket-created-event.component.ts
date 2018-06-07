import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,      
  selector: 'ticket-created-event',
  templateUrl: './ticket-created-event.component.html',
  styleUrls: ['./ticket-created-event.component.css']
})
export class TicketCreatedEventComponent  extends TicketEventComponent  {
  constructor() {
    super();
    this.icon = ['fa', 'fa-bookmark'];
 }
}
