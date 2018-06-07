import { TicketDetail } from "../../entities/read/ticketDetail";
import * as TicketDetailActions from "../actions/ticketDetail";
import * as SharedActions from "../actions/sharedTicket";

export function ticketDetailsReducer(state: Array<TicketDetail> = [], action: TicketDetailActions.Actions): Array<TicketDetail>
{
    switch(action.type)
    {
        case TicketDetailActions.ADD_TICKET_DETAILS:
            return [...state, action.payload];
        case SharedActions.ADD_TICKET:
            return [...state, action.payload];
        case SharedActions.UPDATE_TICKET_STATUS:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { status: action.payload.toStatus, events: [action.payload, ...ticket.events]}) : ticket); 
        case SharedActions.UPDATE_TICKET_PRIORITY:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { priority: action.payload.toPriority, events: [action.payload, ...ticket.events]}) : ticket);    
        case SharedActions.ADD_TICKET_COMMENT:
            return state.map(ticket => ticket.id === action.payload.ticketId ? Object.assign({}, ticket, { events: [action.payload, ...ticket.events] }) : ticket);  
        default:
            return state; 
    }
}