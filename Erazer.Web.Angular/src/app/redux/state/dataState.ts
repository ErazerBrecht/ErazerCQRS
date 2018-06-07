import { IEvent } from '../../entities/read/interfaces/iEvent';
import { Ticket } from '../../entities/read/ticket';
import { TicketDetail } from '../../entities/read/ticketDetail';

export interface DataState{
    readonly tickets: Array<Ticket>;
    readonly ticketDetails: Array<TicketDetail>;
}