import { Component, OnDestroy, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { CreateTicket } from '../../entities/write/createTicket';
import { CreateTicketService } from './create-ticket.service';
import { PriorityValues } from '../../configuration/priorityConstants';
import { State } from '../../redux/state/state';
import * as TicketDetailSelectors from "../../redux/selectors/ticketDetail.selector";
import { Subscription, Observable, pipe } from 'rxjs';
import { take, tap, mergeMap, map, startWith } from 'rxjs/operators';
import { TicketCreateModalComponent } from '../../components/ticket-create-modal/ticket-create-modal.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.css']
})
export class CreateTicketComponent {
  @ViewChild('modal') modal: TicketCreateModalComponent;

  today = new Date();
  priorityValues = PriorityValues;
  loaded$: Observable<boolean>;

  private id: string;

  constructor(private router: Router, private createService: CreateTicketService, private store: Store<State>) { }

  onSave(ticket: CreateTicket): void {
    this.modal.show();

    this.loaded$ = this.createService.add(ticket).pipe(
      take(1),
      tap<string>(id => this.id = id),
      mergeMap(id => this.store.select(TicketDetailSelectors.getTicketDetail(id))),
      map(t => t !== undefined),
      startWith(false)
    );
  }

  onRedirectToTicket() {
    this.router.navigate(['/tickets/detail', this.id]);
  }
}
