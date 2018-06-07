import { Ticket } from "../../entities/read/ticket";
import { TicketFactory } from "../../entities/read/factories/ticketFactory";
import * as TicketActions from "../actions/ticket";
import * as SharedActions from "../actions/sharedTicket";

// TODO Update EventCount and LastUpdated value!
export function ticketsReducer(state: Array<Ticket> = [], action: TicketActions.Actions): Array<Ticket>
{
    switch(action.type)
    {
        case TicketActions.TICKETS_SET_ALL:
            return [...action.payload];
        case SharedActions.ADD_TICKET:
            return [...state, TicketFactory.BuildFromDetail(action.payload)];
        case SharedActions.UPDATE_TICKET_STATUS:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { status: action.payload.toStatus}) : ticket);   
        case SharedActions.UPDATE_TICKET_PRIORITY:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { priority: action.payload.toPriority}) : ticket);
        case SharedActions.ADD_TICKET_COMMENT:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { }) : ticket);  
        default:
            return state; 
    }
}