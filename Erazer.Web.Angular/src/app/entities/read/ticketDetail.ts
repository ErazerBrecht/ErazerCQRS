import { IEvent } from './interfaces/iEvent';
import { Priority } from "./priority";
import { Status } from "./status";
import { Ticket } from './ticket';

export class TicketDetail {
    id: string;
    title: string;
    priority: Priority;
    status: Status;

    // Details
    description: string;
    //type: Priority;             // TODO => Create 'Type' entity
    events: Array<IEvent>;
}