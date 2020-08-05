import { IEvent } from './interfaces/iEvent';
import { Priority } from "./priority";
import { Status } from "./status";

export class TicketDetail {
    id: string;
    title: string;
    priority: Priority;
    status: Status;

    // Details
    description: string;
    //type: Priority;             // TODO => Create 'Type' entity
    events: Array<IEvent>;
    files: Array<any> 
}