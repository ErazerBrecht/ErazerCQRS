import { Observable, Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../redux/state/state';
import * as TicketDetailSelectors from '../../redux/selectors/ticketDetail.selector';

import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TicketDetail } from '../../entities/read/ticketDetail';
import { DetailTicketService } from './detail-ticket.service';
import { CommentBoxComponent } from '../../components/comment-box/comment-box.component';

@Component({
  selector: 'app-detail-ticket',
  templateUrl: './detail-ticket.component.html',
  styleUrls: ['./detail-ticket.component.css']
})
export class DetailTicketComponent {
  id: string = this.route.snapshot.params['id'];
  ticket$: Observable<TicketDetail> = this.store.select(TicketDetailSelectors.getTicketDetail(this.id));
  
  @ViewChild(CommentBoxComponent) commentBox: CommentBoxComponent;

  constructor(private route: ActivatedRoute, private store: Store<State>, private httpService: DetailTicketService) {
  }

  async onCommentAdded(comment: string) {
    await this.httpService.addComment(this.id, comment);
    this.commentBox.reset();
  }

  async onPriorityChanged(priorityId: string) {
    await this.httpService.updatePriority(this.id, priorityId);
  }

  async onStatusChanged(statusId: string) {
    await this.httpService.updateStatus(this.id, statusId);
  }
}
