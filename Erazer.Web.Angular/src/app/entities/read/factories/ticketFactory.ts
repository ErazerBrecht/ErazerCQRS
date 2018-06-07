import { Ticket } from "../ticket";
import { TicketDetail } from "../ticketDetail";


export abstract class TicketFactory {
    public static BuildFromDetail(ticketDetail: TicketDetail): Ticket {
        const ticket = new Ticket();
        ticket.id = ticketDetail.id;
        ticket.priority = ticketDetail.priority;
        ticket.status = ticketDetail.status;
        ticket.title = ticketDetail.title;
        // ticket.eventCount = ticketDetail.events.length;
        // ticket.lastUpdate = ticketDetail.events.reduce((a: IEvent, b: IEvent) => { return a.created > b.created ? a : b;}).created;

        return ticket;
    }
}