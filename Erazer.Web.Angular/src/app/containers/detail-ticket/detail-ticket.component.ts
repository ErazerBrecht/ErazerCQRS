import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../redux/state/state';
import * as TicketDetailSelectors from '../../redux/selectors/ticketDetail.selector';

import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TicketDetail } from '../../entities/read/ticketDetail';
import { DetailTicketService } from './detail-ticket.service';

@Component({
  selector: 'app-detail-ticket',
  templateUrl: './detail-ticket.component.html',
  styleUrls: ['./detail-ticket.component.css']
})
export class DetailTicketComponent {
  id: string = this.route.snapshot.params['id'];
  ticket$: Observable<TicketDetail> = this.store.select(TicketDetailSelectors.getTicketDetail(this.id));

  constructor(private route: ActivatedRoute, private store: Store<State>, private httpService: DetailTicketService) {
  }

  onCommentAdded(comment: string) {
    this.httpService.addComment(this.id, comment).subscribe();
  }

  onPriorityChanged(priorityId: string) {
    this.httpService.updatePriority(this.id, priorityId).subscribe();
  }

  onStatusChanged(statusId: string) {
    this.httpService.updateStatus(this.id, statusId).subscribe();
  }
}
