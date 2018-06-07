import { Action } from "@ngrx/store";
import { Ticket } from "../../entities/read/ticket";
import * as shared from './sharedTicket';

export const TICKETS_SET_ALL = "TICKETS_SET_ALL";

export class SetAllTickets implements Action {
    readonly type = TICKETS_SET_ALL;

    constructor(public payload: Ticket[]) { }
}

export type Actions =
    shared.Actions
    | SetAllTickets;