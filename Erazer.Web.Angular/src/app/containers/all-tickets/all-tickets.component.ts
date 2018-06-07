import { Store } from "@ngrx/store";
import { Component, OnInit } from '@angular/core';

import { State } from "../../redux/state/state";

@Component({
  selector: 'all-tickets',
  templateUrl: './all-tickets.component.html',
  styleUrls: ['./all-tickets.component.css']
})
export class AllTicketsComponent implements OnInit {
  // Data
  tickets$ = this.store.select((state: State) => state.data.tickets);

  constructor(private store: Store<State>) { }

  ngOnInit() {
  }

}
