import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.css']
})
export class TicketListComponent implements OnInit {
  @Input() tickets;
  
  constructor() { }

  ngOnInit() {
  }

}
