import { Component, OnInit, ChangeDetectionStrategy, Input, SimpleChanges } from '@angular/core';
import { TicketDetail } from '../../entities/read/ticketDetail';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,  
  selector: 'ticket-detail-events',
  templateUrl: './ticket-detail-events.component.html',
  styleUrls: ['./ticket-detail-events.component.css']
})
export class TicketDetailEventsComponent implements OnInit {
  @Input() ticket: TicketDetail;
  
  constructor() { }

  ngOnInit() {
  }
}
