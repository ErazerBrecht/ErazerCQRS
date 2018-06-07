import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component'

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,    
  selector: 'ticket-comment-event',
  templateUrl: './ticket-comment-event.component.html',
  styleUrls: ['./ticket-comment-event.component.css']
})
export class TicketCommentEventComponent extends TicketEventComponent {
  constructor() {
    super();
    this.icon = ['fa', 'fa-comment'];
 }
}
