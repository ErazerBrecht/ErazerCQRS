import { Action } from "@ngrx/store";
import { TicketDetail } from "../../entities/read/ticketDetail";
import * as shared from './sharedTicket';

export const ADD_TICKET_DETAILS = "ADD_TICKET_DETAILS";

export class AddTicketDetails implements Action {
    readonly type = ADD_TICKET_DETAILS;

    constructor(public payload: TicketDetail) { }
}

export type Actions =
    shared.Actions
    | AddTicketDetails;