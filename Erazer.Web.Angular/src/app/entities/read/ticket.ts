import { Priority } from "./priority";
import { Status } from "./status";
import { TicketDetail } from "./ticketDetail";

export class Ticket {
    id: string;
    title: string;
    priority: Priority;
    status: Status;
    // eventCount: number;
    // lastUpdate: Date;
}