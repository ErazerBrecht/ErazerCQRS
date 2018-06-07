import { Store } from "@ngrx/store";
import { Component } from '@angular/core';
import { Subscription } from "rxjs";

import { Ticket } from "./entities/read/ticket";
import { AllTicketsService } from './containers/all-tickets/all-tickets.service'
import { State } from "./redux/state/state";
import { SetAllTickets } from "./redux/actions/ticket";
import { AddTicketDetails } from "./redux/actions/ticketDetail";

import { RealTime } from "./common/realtime";
import { TicketDetail } from "./entities/read/ticketDetail";
import { PriorityEvent } from "./entities/read/events/priorityEvent";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // Placeholder to make it possible to unsubscribe the observables
  private subscriptions: Array<Subscription> = [];

  constructor(private service: AllTicketsService, private store: Store<State>, private realtime: RealTime) { }

  ngOnInit() {
    this.realtime.connect();
    this.subscriptions.push(this.service.all().subscribe((tickets: Array<Ticket>) => {
      this.store.dispatch(new SetAllTickets(tickets));
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

}
