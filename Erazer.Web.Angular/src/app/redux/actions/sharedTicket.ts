import { Action } from "@ngrx/store";
import { IEvent } from "../../entities/read/interfaces/iEvent";
import { TicketDetail } from "../../entities/read/ticketDetail";
import { StatusEvent } from "../../entities/read/events/statusEvent";
import { PriorityEvent } from "../../entities/read/events/priorityEvent";

export const ADD_TICKET = "ADD_TICKET";
export const UPDATE_TICKET_WITH_DETAILS = "UPDATE_TICKET_WITH_DETAILS";
export const UPDATE_TICKET_STATUS = "UPDATE_TICKET_STATUS";
export const UPDATE_TICKET_PRIORITY = "UPDATE_TICKET_PRIORITY";
export const ADD_TICKET_COMMENT = "ADD_TICKET_COMMENT";


export class AddTicket implements Action {
    readonly type = ADD_TICKET;

    constructor(public payload: TicketDetail) { }
}

export class UpdateTicketSatus implements Action {
    readonly type = UPDATE_TICKET_STATUS;

    constructor(public payload: StatusEvent) { }
}

export class UpdateTicketPriority implements Action {
    readonly type = UPDATE_TICKET_PRIORITY;

    constructor(public payload: PriorityEvent) { }
}

export class AddTicketComment implements Action {
    readonly type = ADD_TICKET_COMMENT;

    constructor(public payload: IEvent) { }
}

export type Actions =
    AddTicket
    | UpdateTicketSatus
    | UpdateTicketPriority
    | AddTicketComment;