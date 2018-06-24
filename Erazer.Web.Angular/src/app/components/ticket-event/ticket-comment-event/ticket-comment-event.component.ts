import { Component, ChangeDetectionStrategy } from '@angular/core';
import { TicketEventComponent } from '../ticket-event.component'
import { CommentEvent } from '../../../entities/read/events/commentEvent';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'ticket-comment-event',
  templateUrl: './ticket-comment-event.component.html',
  styleUrls: ['./ticket-comment-event.component.css']
})
export class TicketCommentEventComponent extends TicketEventComponent<CommentEvent> {
  commentEvent: CommentEvent;

  constructor() {
    super();
    this.icon = ['fa', 'fa-comment'];
  }
}
